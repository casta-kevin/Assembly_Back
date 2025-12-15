namespace AssemblyApi.Application.DTOs;

public record CreateAgendaQuestionDto
{
    public string Title { get; init; } = string.Empty;
    public string? Description { get; init; }
    public int OrderIndex { get; init; }
    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }
    public List<CreateQuestionOptionDto> Options { get; init; } = new();
}
