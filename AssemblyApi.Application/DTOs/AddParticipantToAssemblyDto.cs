namespace AssemblyApi.Application.DTOs;

public record AddParticipantToAssemblyDto
{
    public Guid AssemblyId { get; init; }
    public Guid UserId { get; init; }
    public bool IsVotingMember { get; init; } = true;
    public bool CanVoteToStartAssembly { get; init; } = false;
}
