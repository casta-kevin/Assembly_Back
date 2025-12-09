using AssemblyApi.Application.DTOs;
using MediatR;

namespace AssemblyApi.Application.UseCases.Commands;

public record AddParticipantToAssembly(AddParticipantToAssemblyDto Data) : IRequest<Guid>;
