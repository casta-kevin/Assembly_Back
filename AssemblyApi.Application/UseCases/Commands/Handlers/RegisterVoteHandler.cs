using AssemblyApi.Application.Constants;
using AssemblyApi.Application.DTOs;
using AssemblyApi.Application.Repositories;
using AssemblyApi.Application.Services;
using AssemblyApi.Domain.Entities;
using AssemblyApi.Domain.Exceptions;
using MediatR;

namespace AssemblyApi.Application.UseCases.Commands.Handlers;

public class RegisterVoteHandler : IRequestHandler<RegisterVote, ApiResponse<Guid>>
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

    public async Task<ApiResponse<Guid>> Handle(RegisterVote request, CancellationToken cancellationToken)
    {
        try
        {
            var data = request.Data;
            var userId = _currentUserService.GetUserId();

            var assembly = await _assemblyRepository.GetByIdAsync(data.AssemblyId, cancellationToken);
            if (assembly == null)
                return ApiResponse<Guid>.FailureResponse("La asamblea no existe");

            if (!assembly.IsInProgress())
                return ApiResponse<Guid>.FailureResponse("La asamblea no está en progreso");

            var question = await _questionRepository.GetByIdAsync(data.QuestionId, cancellationToken);
            if (question == null)
                return ApiResponse<Guid>.FailureResponse("La pregunta no existe");

            if (question.AssemblyId != data.AssemblyId)
                return ApiResponse<Guid>.FailureResponse("La pregunta no pertenece a esta asamblea");

            var isQuestionActive = await _questionRepository.IsQuestionActiveAsync(data.QuestionId, cancellationToken);
            if (!isQuestionActive)
                return ApiResponse<Guid>.FailureResponse("La pregunta no está activa o ya fue procesada");

            if (question.StartDate.HasValue && DateTime.UtcNow < question.StartDate.Value)
                return ApiResponse<Guid>.FailureResponse("La votación para esta pregunta aún no ha iniciado");

            if (question.EndDate.HasValue && DateTime.UtcNow > question.EndDate.Value)
                return ApiResponse<Guid>.FailureResponse("La votación para esta pregunta ya finalizó");

            var confirmedParticipant = await _confirmedParticipantRepository.GetByAssemblyAndUserAsync(
                data.AssemblyId,
                userId,
                cancellationToken);

            if (confirmedParticipant == null)
                return ApiResponse<Guid>.FailureResponse("El usuario no es un participante confirmado de esta asamblea");

            var hasVoted = await _voteRepository.HasVotedAsync(data.QuestionId, userId, cancellationToken);
            if (hasVoted)
                return ApiResponse<Guid>.FailureResponse("El usuario ya ha votado en esta pregunta");

            var questionVoteType = await _voteTypeRepository.GetByIdAsync(VoteTypeIds.Question, cancellationToken);
            if (questionVoteType is null)
                return ApiResponse<Guid>.FailureResponse("El tipo de voto 'QSTN' no esta configurado");

            var vote = new AssemblyVote(
                data.AssemblyId,
                confirmedParticipant.Id,
                questionVoteType.Id,
                data.QuestionId,
                data.OptionId);

            await _voteRepository.AddAsync(vote, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return ApiResponse<Guid>.SuccessResponse(vote.Id, "Voto registrado correctamente");
        }
        catch (Exception ex)
        {
            return ApiResponse<Guid>.FailureResponse(ex.Message);
        }
    }
}
