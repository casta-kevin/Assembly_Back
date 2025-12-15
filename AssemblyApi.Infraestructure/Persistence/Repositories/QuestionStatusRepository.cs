using AssemblyApi.Application.Repositories;
using AssemblyApi.Domain.Entities;
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

    public async Task<QuestionStatus?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _context.QuestionStatuses
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }
}
