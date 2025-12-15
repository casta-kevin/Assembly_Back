using AssemblyApi.Domain.Entities;

namespace AssemblyApi.Application.Repositories;

public interface IVoteTypeRepository
{
    Task<VoteType?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
}
