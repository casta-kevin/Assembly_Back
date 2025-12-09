namespace AssemblyApi.Application.DTOs;

public record RegisterUserResponseDto
{
    public Guid UserId { get; init; }
    public string Username { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public Guid PropertyId { get; init; }
}
