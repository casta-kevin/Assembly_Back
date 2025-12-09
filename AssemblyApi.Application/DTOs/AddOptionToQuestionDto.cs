namespace AssemblyApi.Application.DTOs;

public record AddOptionToQuestionDto
{
    public Guid QuestionId { get; init; }
    public string Text { get; init; } = string.Empty;
    public int OrderIndex { get; init; }
}
