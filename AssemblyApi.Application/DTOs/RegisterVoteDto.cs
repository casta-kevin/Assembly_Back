namespace AssemblyApi.Application.DTOs;

public record RegisterVoteDto
{
    public Guid AssemblyId { get; init; }
    public Guid QuestionId { get; init; }
    public Guid OptionId { get; init; }
}
