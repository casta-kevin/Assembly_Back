using AssemblyApi.Domain.Entities;

namespace AssemblyApi.Application.Repositories;

public interface IParticipantRepository
{
    Task AddAsync(AssemblyParticipant participant, CancellationToken cancellationToken = default);
    Task<AssemblyParticipant?> GetByAssemblyAndUserAsync(Guid assemblyId, Guid userId, CancellationToken cancellationToken = default);
}
