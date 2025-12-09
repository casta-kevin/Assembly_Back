using AssemblyApi.Domain.Common;

namespace AssemblyApi.Domain.Entities;

public class AssemblyVote : Entity
{
    public Guid AssemblyId { get; private set; }
    public Guid? QuestionId { get; private set; }
    public Guid? OptionId { get; private set; }
    public Guid ConfirmedParticipantId { get; private set; }
    public Guid VoteTypeId { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private AssemblyVote() { }

    public AssemblyVote(Guid assemblyId, Guid confirmedParticipantId, Guid voteTypeId, Guid? questionId = null, Guid? optionId = null)
    {
        AssemblyId = assemblyId;
        ConfirmedParticipantId = confirmedParticipantId;
        VoteTypeId = voteTypeId;
        QuestionId = questionId;
        OptionId = optionId;
        CreatedAt = DateTime.UtcNow;
    }
}
