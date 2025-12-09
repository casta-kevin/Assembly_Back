using AssemblyApi.Domain.Common;
using AssemblyApi.Domain.Exceptions;

namespace AssemblyApi.Domain.Entities;

public class AssemblyQuestionResult : Entity
{
    public Guid AssemblyId { get; private set; }
    public Guid QuestionId { get; private set; }
    public Guid OptionId { get; private set; }
    public int VotesCount { get; private set; }
    public bool IsWinningOption { get; private set; }
    public bool IsTie { get; private set; }
    public DateTime CalculatedAt { get; private set; }

    private AssemblyQuestionResult() { }

    public AssemblyQuestionResult(Guid assemblyId, Guid questionId, Guid optionId, int votesCount)
    {
        if (assemblyId == Guid.Empty)
            throw new DomainException("La asamblea es requerida");

        if (questionId == Guid.Empty)
            throw new DomainException("La pregunta es requerida");

        if (optionId == Guid.Empty)
            throw new DomainException("La opción es requerida");

        if (votesCount < 0)
            throw new DomainException("El conteo de votos no puede ser negativo");

        AssemblyId = assemblyId;
        QuestionId = questionId;
        OptionId = optionId;
        VotesCount = votesCount;
        IsWinningOption = false;
        IsTie = false;
        CalculatedAt = DateTime.UtcNow;
    }

    public void MarkAsWinner()
    {
        if (IsTie)
            throw new DomainException("No se puede marcar como ganador una opción que está en empate");

        IsWinningOption = true;
    }

    public void MarkAsTie()
    {
        if (IsWinningOption)
            throw new DomainException("No se puede marcar como empate una opción que ya es ganadora");

        IsTie = true;
        IsWinningOption = false;
    }

    public void UpdateVotesCount(int newCount)
    {
        if (newCount < 0)
            throw new DomainException("El conteo de votos no puede ser negativo");

        VotesCount = newCount;
        CalculatedAt = DateTime.UtcNow;
    }
}
