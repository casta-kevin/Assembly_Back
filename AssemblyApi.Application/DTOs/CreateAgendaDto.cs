namespace AssemblyApi.Application.DTOs;

public record CreateAgendaDto
{
    public Guid AssemblyId { get; init; }
    public List<CreateAgendaQuestionDto> Questions { get; init; } = new();
}
