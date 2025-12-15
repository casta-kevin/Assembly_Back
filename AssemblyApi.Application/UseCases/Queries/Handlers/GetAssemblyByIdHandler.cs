using AssemblyApi.Application.DTOs;
using AssemblyApi.Application.Repositories;
using MediatR;

namespace AssemblyApi.Application.UseCases.Queries.Handlers;

public class GetAssemblyByIdHandler : IRequestHandler<GetAssemblyById, ApiResponse<AssemblyDto?>>
{
    private readonly IAssemblyRepository _assemblyRepository;

    public GetAssemblyByIdHandler(IAssemblyRepository assemblyRepository)
    {
        _assemblyRepository = assemblyRepository;
    }

    public async Task<ApiResponse<AssemblyDto?>> Handle(GetAssemblyById request, CancellationToken cancellationToken)
    {
        try
        {
            var assembly = await _assemblyRepository.GetByIdAsync(request.Id, cancellationToken);

            if (assembly == null)
                return ApiResponse<AssemblyDto?>.FailureResponse("La asamblea no existe");

            var dto = new AssemblyDto
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

            return ApiResponse<AssemblyDto?>.SuccessResponse(dto);
        }
        catch (Exception ex)
        {
            return ApiResponse<AssemblyDto?>.FailureResponse("Error al obtener la asamblea", new[] { ex.Message });
        }
    }
}
