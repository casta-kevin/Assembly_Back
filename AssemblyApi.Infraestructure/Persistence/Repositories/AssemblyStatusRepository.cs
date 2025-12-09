using AssemblyApi.Application.Repositories;
using AssemblyApi.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AssemblyApi.Infraestructure.Persistence.Repositories;

public class AssemblyStatusRepository : IAssemblyStatusRepository
{
    private readonly ApplicationDbContext _context;

    public AssemblyStatusRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        var status = await _context.AssemblyStatuses
            .FirstOrDefaultAsync(s => s.Code == code, cancellationToken);

        return status?.Id ?? Guid.Empty;
    }
}
