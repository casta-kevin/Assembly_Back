namespace AssemblyApi.Application.DTOs;

public record LoginResponseDto
{
    public string Token { get; init; } = string.Empty;
    public Guid UserId { get; init; }
    public Guid PropertyId { get; init; }
    public string Username { get; init; } = string.Empty;
    public string RoleId { get; init; } = string.Empty;
}
