namespace AssemblyApi.Application.DTOs;

public record AddQuestionToAssemblyDto
{
    public Guid AssemblyId { get; init; }
    public string Title { get; init; } = string.Empty;
    public string? Description { get; init; }
    public string QuestionSourceId { get; init; } = string.Empty;
    public int? OrderIndex { get; init; }
    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }
    public List<CreateQuestionOptionDto> Options { get; init; } = new();
}
