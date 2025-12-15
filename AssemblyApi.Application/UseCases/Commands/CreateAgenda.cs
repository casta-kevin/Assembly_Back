using AssemblyApi.Application.DTOs;
using MediatR;

namespace AssemblyApi.Application.UseCases.Commands;

public record CreateAgenda(CreateAgendaDto Data) : IRequest<ApiResponse<List<Guid>>>;
