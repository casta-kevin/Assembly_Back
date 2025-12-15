using AssemblyApi.Application.DTOs;
using MediatR;

namespace AssemblyApi.Application.UseCases.Queries;

public record GetAssemblyById(Guid Id) : IRequest<ApiResponse<AssemblyDto?>>;
