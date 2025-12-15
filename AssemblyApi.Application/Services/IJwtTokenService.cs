namespace AssemblyApi.Application.Services;

public interface IJwtTokenService
{
    string GenerateToken(Guid userId, Guid propertyId, string username, string roleId);
}
