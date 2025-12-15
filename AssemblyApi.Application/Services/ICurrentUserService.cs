namespace AssemblyApi.Application.Services;

public interface ICurrentUserService
{
    Guid GetUserId();
    Guid GetPropertyId();
    string GetUsername();
    string GetRoleId();
}
