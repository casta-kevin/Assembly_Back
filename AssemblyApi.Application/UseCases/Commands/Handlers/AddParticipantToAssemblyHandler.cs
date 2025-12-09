using AssemblyApi.Application.Repositories;
using AssemblyApi.Domain.Entities;
using AssemblyApi.Domain.Exceptions;
using MediatR;

namespace AssemblyApi.Application.UseCases.Commands.Handlers;

public class AddParticipantToAssemblyHandler : IRequestHandler<AddParticipantToAssembly, Guid>
{
    private readonly IParticipantRepository _participantRepository;
    private readonly IAssemblyRepository _assemblyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddParticipantToAssemblyHandler(
        IParticipantRepository participantRepository,
        IAssemblyRepository assemblyRepository,
        IUnitOfWork unitOfWork)
    {
        _participantRepository = participantRepository;
        _assemblyRepository = assemblyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(AddParticipantToAssembly request, CancellationToken cancellationToken)
    {
        var data = request.Data;

        var assembly = await _assemblyRepository.GetByIdAsync(data.AssemblyId, cancellationToken);
        
        if (assembly == null)
            throw new DomainException("La asamblea no existe");

        if (assembly.IsInProgress() || assembly.IsClosed())
            throw new InvalidAssemblyStateException("No se pueden agregar participantes a una asamblea iniciada o cerrada");

        var existingParticipant = await _participantRepository.GetByAssemblyAndUserAsync(
            data.AssemblyId, 
            data.UserId, 
            cancellationToken);

        if (existingParticipant != null)
            throw new DomainException("El participante ya está registrado");

        var participant = new AssemblyParticipant(
            data.AssemblyId,
            data.UserId,
            data.IsVotingMember
        );

        if (data.CanVoteToStartAssembly)
        {
            participant.AllowStartAssemblyVote();
        }

        await _participantRepository.AddAsync(participant, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return participant.Id;
    }
}
