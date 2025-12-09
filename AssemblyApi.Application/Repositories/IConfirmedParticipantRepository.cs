using AssemblyApi.Domain.Entities;

namespace AssemblyApi.Application.Repositories;

public interface IConfirmedParticipantRepository
{
    Task<AssemblyConfirmedParticipant?> GetByAssemblyAndUserAsync(Guid assemblyId, Guid userId, CancellationToken cancellationToken = default);
}
