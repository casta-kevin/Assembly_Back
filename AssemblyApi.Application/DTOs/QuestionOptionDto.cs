namespace AssemblyApi.Application.DTOs;

public record QuestionOptionDto
{
    public Guid Id { get; init; }
    public string Text { get; init; } = string.Empty;
    public int OrderIndex { get; init; }
}
