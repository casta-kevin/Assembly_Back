namespace AssemblyApi.Application.Repositories;

public interface IVoteTypeRepository
{
    Task<Guid> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
}
