namespace AssemblyApi.Application.DTOs;

public record LoginDto
{
    public string Username { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}
