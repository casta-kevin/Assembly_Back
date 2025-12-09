using AssemblyApi.Application.Repositories;
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

    public async Task<Guid> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        var voteType = await _context.VoteTypes
            .FirstOrDefaultAsync(v => v.Code == code, cancellationToken);

        return voteType?.Id ?? Guid.Empty;
    }
}
