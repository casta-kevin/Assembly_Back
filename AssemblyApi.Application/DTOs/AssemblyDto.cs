namespace AssemblyApi.Application.DTOs;

public record AssemblyDto
{
    public Guid Id { get; init; }
    public Guid PropertyId { get; init; }
    public string Title { get; init; } = string.Empty;
    public string? Description { get; init; }
    public string? Rules { get; init; }
    public DateTime? StartDatePlanned { get; init; }
    public DateTime? EndDatePlanned { get; init; }
    public DateTime? StartDateActual { get; init; }
    public DateTime? EndDateActual { get; init; }
    public string Status { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
}
