using AssemblyApi.Application.DTOs;
using AssemblyApi.Application.Repositories;
using MediatR;

namespace AssemblyApi.Application.UseCases.Queries.Handlers;

public class GetAssemblyByIdHandler : IRequestHandler<GetAssemblyById, AssemblyDto?>
{
    private readonly IAssemblyRepository _assemblyRepository;
    private readonly IAssemblyStatusRepository _statusRepository;

    public GetAssemblyByIdHandler(
        IAssemblyRepository assemblyRepository,
        IAssemblyStatusRepository statusRepository)
    {
        _assemblyRepository = assemblyRepository;
        _statusRepository = statusRepository;
    }

    public async Task<AssemblyDto?> Handle(GetAssemblyById request, CancellationToken cancellationToken)
    {
        var assembly = await _assemblyRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (assembly == null)
            return null;

        var status = await _statusRepository.GetByCodeAsync(assembly.AssemblyStatusId.ToString(), cancellationToken);

        return new AssemblyDto
        {
            Id = assembly.Id,
            PropertyId = assembly.PropertyId,
            Title = assembly.Title,
            Description = assembly.Description,
            Rules = assembly.Rules,
            StartDatePlanned = assembly.StartDatePlanned,
            EndDatePlanned = assembly.EndDatePlanned,
            StartDateActual = assembly.StartDateActual,
            EndDateActual = assembly.EndDateActual,
            Status = assembly.IsInProgress() ? "In Progress" : assembly.IsClosed() ? "Closed" : "Scheduled",
            CreatedAt = assembly.CreatedAt
        };
    }
}
