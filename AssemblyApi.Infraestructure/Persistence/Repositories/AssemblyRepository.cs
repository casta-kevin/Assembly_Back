using AssemblyApi.Application.Repositories;
using AssemblyApi.Domain.Entities;
using AssemblyApi.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AssemblyApi.Infraestructure.Persistence.Repositories;

public class AssemblyRepository : IAssemblyRepository
{
    private readonly ApplicationDbContext _context;

    public AssemblyRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Assembly assembly, CancellationToken cancellationToken = default)
    {
        await _context.Assemblies.AddAsync(assembly, cancellationToken);
    }

    public async Task<Assembly?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Assemblies
            .Include(a => a.Participants)
            .Include(a => a.Questions)
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }
}
