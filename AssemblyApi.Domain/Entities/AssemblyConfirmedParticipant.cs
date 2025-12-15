using AssemblyApi.Domain.Common;
using AssemblyApi.Domain.Exceptions;

namespace AssemblyApi.Domain.Entities;

public class AssemblyConfirmedParticipant : Entity
{
    public Guid AssemblyId { get; private set; }
    public Guid ParticipantId { get; private set; }
    public DateTime ConfirmedAt { get; private set; }
    public Guid? ConfirmedByUserId { get; private set; }
    public string ConfirmationMethodId { get; private set; } = string.Empty;

    private AssemblyConfirmedParticipant() { }

    public AssemblyConfirmedParticipant(Guid assemblyId, Guid participantId, string confirmationMethodId, Guid? confirmedByUserId = null)
    {
        if (assemblyId == Guid.Empty)
            throw new DomainException("La asamblea es requerida");

        if (participantId == Guid.Empty)
            throw new DomainException("El participante es requerido");

        if (string.IsNullOrWhiteSpace(confirmationMethodId))
            throw new DomainException("El método de confirmación es requerido");

        AssemblyId = assemblyId;
        ParticipantId = participantId;
        ConfirmationMethodId = confirmationMethodId;
        ConfirmedByUserId = confirmedByUserId;
        ConfirmedAt = DateTime.UtcNow;
    }
}
