using System.Security.Claims;

namespace AssemblyApi.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        var userIdClaim = user.FindFirst("UserId")?.Value;
        return Guid.TryParse(userIdClaim, out var userId) ? userId : Guid.Empty;
    }

    public static Guid GetPropertyId(this ClaimsPrincipal user)
    {
        var propertyIdClaim = user.FindFirst("PropertyId")?.Value;
        return Guid.TryParse(propertyIdClaim, out var propertyId) ? propertyId : Guid.Empty;
    }

    public static string GetUsername(this ClaimsPrincipal user)
    {
        return user.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
    }
}
