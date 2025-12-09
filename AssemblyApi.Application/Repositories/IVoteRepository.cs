using AssemblyApi.Domain.Entities;

namespace AssemblyApi.Application.Repositories;

public interface IVoteRepository
{
    Task AddAsync(AssemblyVote vote, CancellationToken cancellationToken = default);
    Task<bool> HasVotedAsync(Guid questionId, Guid userId, CancellationToken cancellationToken = default);
    Task<int> GetVoteCountByOptionAsync(Guid questionId, Guid optionId, CancellationToken cancellationToken = default);
}
