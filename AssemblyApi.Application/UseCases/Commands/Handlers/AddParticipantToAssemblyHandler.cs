using AssemblyApi.Application.DTOs;
using AssemblyApi.Application.Repositories;
using AssemblyApi.Domain.Entities;
using MediatR;

namespace AssemblyApi.Application.UseCases.Commands.Handlers;

public class AddParticipantToAssemblyHandler : IRequestHandler<AddParticipantToAssembly, ApiResponse<Guid>>
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

    public async Task<ApiResponse<Guid>> Handle(AddParticipantToAssembly request, CancellationToken cancellationToken)
    {
        try
        {
            var data = request.Data;

            var assembly = await _assemblyRepository.GetByIdAsync(data.AssemblyId, cancellationToken);

            if (assembly == null)
                return ApiResponse<Guid>.FailureResponse("La asamblea no existe");

            if (assembly.IsInProgress() || assembly.IsClosed())
                return ApiResponse<Guid>.FailureResponse("No se pueden agregar participantes a una asamblea iniciada o cerrada");

            if (data.UserId == Guid.Empty)
                return ApiResponse<Guid>.FailureResponse("El usuario es requerido");

            var participantExists = await _participantRepository.GetByAssemblyAndUserAsync(data.AssemblyId, data.UserId, cancellationToken);
            if (participantExists != null)
                return ApiResponse<Guid>.FailureResponse("El participante ya está registrado");

            var participant = new AssemblyParticipant(data.AssemblyId, data.UserId, data.IsVotingMember);

            if (data.CanVoteToStartAssembly)
            {
                participant.AllowStartAssemblyVote();
            }

            await _participantRepository.AddAsync(participant, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return ApiResponse<Guid>.SuccessResponse(participant.Id, "Participante agregado correctamente");
        }
        catch (Exception ex)
        {
            return ApiResponse<Guid>.FailureResponse(ex.Message);
        }
    }
}
