namespace AssemblyApi.Application.DTOs;

public record CreateAssemblyDto
{
    public string Title { get; init; } = string.Empty;
    public string? Description { get; init; }
    public string? Rules { get; init; }
    public DateTime? StartDatePlanned { get; init; }
    public DateTime? EndDatePlanned { get; init; }
}
