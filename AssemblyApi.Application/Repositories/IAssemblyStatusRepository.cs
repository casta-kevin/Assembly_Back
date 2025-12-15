using AssemblyApi.Domain.Entities;

namespace AssemblyApi.Application.Repositories;

public interface IAssemblyStatusRepository
{
    Task<AssemblyStatus?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
}
