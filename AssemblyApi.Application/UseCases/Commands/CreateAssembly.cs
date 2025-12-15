using AssemblyApi.Application.DTOs;
using MediatR;

namespace AssemblyApi.Application.UseCases.Commands;

public record CreateAssembly(CreateAssemblyDto Data) : IRequest<ApiResponse<Guid>>;
