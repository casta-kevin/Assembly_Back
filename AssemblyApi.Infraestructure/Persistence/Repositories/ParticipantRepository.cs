using AssemblyApi.Application.Repositories;
using AssemblyApi.Domain.Entities;
using AssemblyApi.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AssemblyApi.Infraestructure.Persistence.Repositories;

public class ParticipantRepository : IParticipantRepository
{
    private readonly ApplicationDbContext _context;

    public ParticipantRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(AssemblyParticipant participant, CancellationToken cancellationToken = default)
    {
        await _context.AssemblyParticipants.AddAsync(participant, cancellationToken);
    }

    public async Task<AssemblyParticipant?> GetByAssemblyAndUserAsync(Guid assemblyId, Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.AssemblyParticipants
            .FirstOrDefaultAsync(p => p.AssemblyId == assemblyId && p.UserId == userId, cancellationToken);
    }
}
