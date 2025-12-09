using AssemblyApi.Application.Services;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace AssemblyApi.Infraestructure.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid GetUserId()
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("UserId")?.Value;
        return Guid.TryParse(userIdClaim, out var userId) ? userId : Guid.Empty;
    }

    public Guid GetPropertyId()
    {
        var propertyIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("PropertyId")?.Value;
        return Guid.TryParse(propertyIdClaim, out var propertyId) ? propertyId : Guid.Empty;
    }

    public string GetUsername()
    {
        return _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
    }
}
