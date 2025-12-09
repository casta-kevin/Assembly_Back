namespace AssemblyApi.Application.Repositories;

public interface IAssemblyStatusRepository
{
    Task<Guid> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
}
