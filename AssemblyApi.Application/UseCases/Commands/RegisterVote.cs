using AssemblyApi.Application.DTOs;
using MediatR;

namespace AssemblyApi.Application.UseCases.Commands;

public record RegisterVote(RegisterVoteDto Data) : IRequest<Guid>;
