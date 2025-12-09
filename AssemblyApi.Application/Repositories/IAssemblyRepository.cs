using Entity = AssemblyApi.Domain.Entities.Assembly;

namespace AssemblyApi.Application.Repositories;

public interface IAssemblyRepository
{
    Task AddAsync(Entity assembly, CancellationToken cancellationToken = default);
    Task<Entity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
