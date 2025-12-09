using AssemblyApi.Application.Repositories;
using AssemblyApi.Application.Services;
using AssemblyApi.Domain.Entities;
using AssemblyApi.Domain.Exceptions;
using MediatR;

namespace AssemblyApi.Application.UseCases.Commands.Handlers;

public class RegisterVoteHandler : IRequestHandler<RegisterVote, Guid>
{
    private readonly IVoteRepository _voteRepository;
    private readonly IQuestionRepository _questionRepository;
    private readonly IAssemblyRepository _assemblyRepository;
    private readonly IConfirmedParticipantRepository _confirmedParticipantRepository;
    private readonly IVoteTypeRepository _voteTypeRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterVoteHandler(
        IVoteRepository voteRepository,
        IQuestionRepository questionRepository,
        IAssemblyRepository assemblyRepository,
        IConfirmedParticipantRepository confirmedParticipantRepository,
        IVoteTypeRepository voteTypeRepository,
        ICurrentUserService currentUserService,
        IUnitOfWork unitOfWork)
    {
        _voteRepository = voteRepository;
        _questionRepository = questionRepository;
        _assemblyRepository = assemblyRepository;
        _confirmedParticipantRepository = confirmedParticipantRepository;
        _voteTypeRepository = voteTypeRepository;
        _currentUserService = currentUserService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(RegisterVote request, CancellationToken cancellationToken)
    {
        var data = request.Data;
        var userId = _currentUserService.GetUserId();

        var assembly = await _assemblyRepository.GetByIdAsync(data.AssemblyId, cancellationToken);
        if (assembly == null)
            throw new DomainException("La asamblea no existe");

        if (!assembly.IsInProgress())
            throw new InvalidAssemblyStateException("La asamblea no está en progreso");

        var question = await _questionRepository.GetByIdAsync(data.QuestionId, cancellationToken);
        if (question == null)
            throw new DomainException("La pregunta no existe");

        if (question.AssemblyId != data.AssemblyId)
            throw new DomainException("La pregunta no pertenece a esta asamblea");

        var isQuestionActive = await _questionRepository.IsQuestionActiveAsync(data.QuestionId, cancellationToken);
        if (!isQuestionActive)
            throw new DomainException("La pregunta no está activa o ya fue procesada");

        if (question.StartDate.HasValue && DateTime.UtcNow < question.StartDate.Value)
            throw new DomainException("La votación para esta pregunta aún no ha iniciado");

        if (question.EndDate.HasValue && DateTime.UtcNow > question.EndDate.Value)
            throw new DomainException("La votación para esta pregunta ya finalizó");

        var confirmedParticipant = await _confirmedParticipantRepository.GetByAssemblyAndUserAsync(
            data.AssemblyId, 
            userId, 
            cancellationToken);

        if (confirmedParticipant == null)
            throw new DomainException("El usuario no es un participante confirmado de esta asamblea");

        var hasVoted = await _voteRepository.HasVotedAsync(data.QuestionId, userId, cancellationToken);
        if (hasVoted)
            throw new DomainException("El usuario ya ha votado en esta pregunta");

        var questionVoteTypeId = await _voteTypeRepository.GetByCodeAsync("QUESTION", cancellationToken);

        var vote = new AssemblyVote(
            data.AssemblyId,
            confirmedParticipant.Id,
            questionVoteTypeId,
            data.QuestionId,
            data.OptionId
        );

        await _voteRepository.AddAsync(vote, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return vote.Id;
    }
}
