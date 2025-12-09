using AssemblyApi.Application.Repositories;
using AssemblyApi.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AssemblyApi.Infraestructure.Persistence.Repositories;

public class QuestionStatusRepository : IQuestionStatusRepository
{
    private readonly ApplicationDbContext _context;

    public QuestionStatusRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        var status = await _context.QuestionStatuses
            .FirstOrDefaultAsync(s => s.Code == code, cancellationToken);

        return status?.Id ?? Guid.Empty;
    }
}
