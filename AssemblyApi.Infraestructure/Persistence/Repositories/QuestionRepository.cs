using AssemblyApi.Application.DTOs;
using AssemblyApi.Application.Repositories;
using AssemblyApi.Application.Common;
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
            .Include(q => q.Options)
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

        if (status == null)
            return false;

        return status.Id is "INPR" or "PLND";
    }

    public async Task<List<AssemblyQuestion>> GetByAssemblyIdAsync(Guid assemblyId, QuestionQueryParametersDto parameters, CancellationToken cancellationToken = default)
    {
        var safeParams = parameters ?? new QuestionQueryParametersDto();

        var query = _context.AssemblyQuestions
            .Include(q => q.Options)
            .Where(q => q.AssemblyId == assemblyId);

        var filterBuilder = QueryFilterBuilder<AssemblyQuestion>.Create()
            .Add(!string.IsNullOrWhiteSpace(safeParams.Search), q => q.Where(x =>
                EF.Functions.ILike(x.Title, $"%{safeParams.Search!}%") ||
                (x.Description != null && EF.Functions.ILike(x.Description, $"%{safeParams.Search}%"))))
            .Add(!string.IsNullOrWhiteSpace(safeParams.StatusId), q => q.Where(x => x.QuestionStatusId == safeParams.StatusId))
            .Add(!string.IsNullOrWhiteSpace(safeParams.SourceId), q => q.Where(x => x.QuestionSourceId == safeParams.SourceId))
            .Add(safeParams.StartDateFrom.HasValue, q => q.Where(x => x.StartDate >= safeParams.StartDateFrom))
            .Add(safeParams.StartDateTo.HasValue, q => q.Where(x => x.StartDate <= safeParams.StartDateTo))
            .Add(safeParams.EndDateFrom.HasValue, q => q.Where(x => x.EndDate >= safeParams.EndDateFrom))
            .Add(safeParams.EndDateTo.HasValue, q => q.Where(x => x.EndDate <= safeParams.EndDateTo));

        query = filterBuilder.Apply(query);

        var page = safeParams.Page <= 0 ? 1 : safeParams.Page;
        var size = safeParams.PageSize <= 0 ? 20 : Math.Min(safeParams.PageSize, 100);
        var skip = (page - 1) * size;

        query = query
            .OrderBy(q => q.OrderIndex)
            .ThenBy(q => q.CreatedAt)
            .Skip(skip)
            .Take(size);

        return await query.ToListAsync(cancellationToken);
    }
}
