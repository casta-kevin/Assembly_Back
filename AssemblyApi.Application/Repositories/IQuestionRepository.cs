using AssemblyApi.Application.DTOs;
using AssemblyApi.Domain.Entities;

namespace AssemblyApi.Application.Repositories;

public interface IQuestionRepository
{
    Task<AssemblyQuestion?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> IsQuestionActiveAsync(Guid questionId, CancellationToken cancellationToken = default);
    Task<List<AssemblyQuestion>> GetByAssemblyIdAsync(Guid assemblyId, QuestionQueryParametersDto parameters, CancellationToken cancellationToken = default);
}
