using AssemblyApi.Domain.Common;
using AssemblyApi.Domain.Exceptions;

namespace AssemblyApi.Domain.Entities;

public class AssemblyVote : Entity
{
    public Guid AssemblyId { get; private set; }
    public Guid? QuestionId { get; private set; }
    public Guid? OptionId { get; private set; }
    public Guid ConfirmedParticipantId { get; private set; }
    public string VoteTypeId { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }

    private AssemblyVote() { }

    public AssemblyVote(Guid assemblyId, Guid confirmedParticipantId, string voteTypeId, Guid? questionId = null, Guid? optionId = null)
    {
        if (assemblyId == Guid.Empty)
            throw new DomainException("La asamblea es requerida");

        if (confirmedParticipantId == Guid.Empty)
            throw new DomainException("El participante confirmado es requerido");

        if (string.IsNullOrWhiteSpace(voteTypeId))
            throw new DomainException("El tipo de voto es requerido");

        AssemblyId = assemblyId;
        ConfirmedParticipantId = confirmedParticipantId;
        VoteTypeId = voteTypeId;
        QuestionId = questionId;
        OptionId = optionId;
        CreatedAt = DateTime.UtcNow;
    }
}
