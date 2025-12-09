namespace AssemblyApi.Application.DTOs;

public record RegisterUserDto
{
    public string Username { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public Guid PropertyId { get; init; }
}
