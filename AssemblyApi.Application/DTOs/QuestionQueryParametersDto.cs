namespace AssemblyApi.Application.DTOs;

public record QuestionQueryParametersDto
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;
    public string? Search { get; init; }
    public string? StatusId { get; init; }
    public string? SourceId { get; init; }
    public DateTime? StartDateFrom { get; init; }
    public DateTime? StartDateTo { get; init; }
    public DateTime? EndDateFrom { get; init; }
    public DateTime? EndDateTo { get; init; }
}
