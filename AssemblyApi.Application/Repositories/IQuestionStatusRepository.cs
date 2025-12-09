namespace AssemblyApi.Application.Repositories;

public interface IQuestionStatusRepository
{
    Task<Guid> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
}
