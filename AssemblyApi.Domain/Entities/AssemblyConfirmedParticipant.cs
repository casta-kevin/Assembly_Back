using AssemblyApi.Domain.Common;

namespace AssemblyApi.Domain.Entities;

public class AssemblyConfirmedParticipant : Entity
{
    public Guid AssemblyId { get; private set; }
    public Guid ParticipantId { get; private set; }
    public DateTime ConfirmedAt { get; private set; }
    public Guid? ConfirmedByUserId { get; private set; }
    public Guid ConfirmationMethodId { get; private set; }

    private AssemblyConfirmedParticipant() { }

    public AssemblyConfirmedParticipant(Guid assemblyId, Guid participantId, Guid confirmationMethodId, Guid? confirmedByUserId = null)
    {
        AssemblyId = assemblyId;
        ParticipantId = participantId;
        ConfirmationMethodId = confirmationMethodId;
        ConfirmedByUserId = confirmedByUserId;
        ConfirmedAt = DateTime.UtcNow;
    }
}
