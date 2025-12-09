namespace AssemblyApi.Application.DTOs;

public record QuestionDto
{
    public Guid Id { get; init; }
    public Guid AssemblyId { get; init; }
    public string Title { get; init; } = string.Empty;
    public string? Description { get; init; }
    public int OrderIndex { get; init; }
    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }
    public List<QuestionOptionDto> Options { get; init; } = new();
}
