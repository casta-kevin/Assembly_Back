using AssemblyApi.Domain.Entities;

namespace AssemblyApi.Application.Repositories;

public interface IQuestionStatusRepository
{
    Task<QuestionStatus?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
}
