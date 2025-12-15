using AssemblyApi.Application.Repositories;
using AssemblyApi.Domain.Entities;
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

    public async Task<AssemblyStatus?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _context.AssemblyStatuses
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }
}
