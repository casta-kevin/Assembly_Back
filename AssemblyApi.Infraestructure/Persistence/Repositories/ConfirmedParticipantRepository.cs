using AssemblyApi.Application.Repositories;
using AssemblyApi.Domain.Entities;
using AssemblyApi.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AssemblyApi.Infraestructure.Persistence.Repositories;

public class ConfirmedParticipantRepository : IConfirmedParticipantRepository
{
    private readonly ApplicationDbContext _context;

    public ConfirmedParticipantRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AssemblyConfirmedParticipant?> GetByAssemblyAndUserAsync(Guid assemblyId, Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.AssemblyConfirmedParticipants
            .Join(_context.AssemblyParticipants,
                confirmed => confirmed.ParticipantId,
                participant => participant.Id,
                (confirmed, participant) => new { confirmed, participant })
            .Where(x => x.confirmed.AssemblyId == assemblyId && x.participant.UserId == userId)
            .Select(x => x.confirmed)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
