using AssemblyApi.Application.Repositories;
using AssemblyApi.Domain.Entities;
using AssemblyApi.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AssemblyApi.Infraestructure.Persistence.Repositories;

public class VoteTypeRepository : IVoteTypeRepository
{
    private readonly ApplicationDbContext _context;

    public VoteTypeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<VoteType?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _context.VoteTypes
            .FirstOrDefaultAsync(v => v.Id == id, cancellationToken);
    }
}
