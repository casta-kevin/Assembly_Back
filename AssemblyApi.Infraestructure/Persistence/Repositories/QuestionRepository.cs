using AssemblyApi.Application.Repositories;
using AssemblyApi.Domain.Entities;
using AssemblyApi.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AssemblyApi.Infraestructure.Persistence.Repositories;

public class QuestionRepository : IQuestionRepository
{
    private readonly ApplicationDbContext _context;

    public QuestionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AssemblyQuestion?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.AssemblyQuestions
            .Include("_options")
            .FirstOrDefaultAsync(q => q.Id == id, cancellationToken);
    }

    public async Task<bool> IsQuestionActiveAsync(Guid questionId, CancellationToken cancellationToken = default)
    {
        var question = await _context.AssemblyQuestions
            .FirstOrDefaultAsync(q => q.Id == questionId, cancellationToken);

        if (question == null)
            return false;

        var status = await _context.QuestionStatuses
            .FirstOrDefaultAsync(s => s.Id == question.QuestionStatusId, cancellationToken);

        return status?.Code == "IN_PROGRESS" || status?.Code == "PLANNED";
    }

    public async Task<List<AssemblyQuestion>> GetByAssemblyIdAsync(Guid assemblyId, CancellationToken cancellationToken = default)
    {
        return await _context.AssemblyQuestions
            .Include("_options")
            .Where(q => q.AssemblyId == assemblyId)
            .OrderBy(q => q.OrderIndex)
            .ToListAsync(cancellationToken);
    }
}
