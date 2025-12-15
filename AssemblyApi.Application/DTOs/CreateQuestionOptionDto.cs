namespace AssemblyApi.Application.DTOs;

public record CreateQuestionOptionDto
{
    public string Text { get; init; } = string.Empty;
}
