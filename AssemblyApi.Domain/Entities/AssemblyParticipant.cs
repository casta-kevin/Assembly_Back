using AssemblyApi.Domain.Common;
using AssemblyApi.Domain.Exceptions;

namespace AssemblyApi.Domain.Entities;

public class AssemblyParticipant : Entity
{
    public Guid AssemblyId { get; private set; }
    public Guid UserId { get; private set; }
    public bool IsVotingMember { get; private set; }
    public bool CanVoteToStartAssembly { get; private set; }
    public bool IsAdministrator { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private AssemblyParticipant() { }

    public AssemblyParticipant(Guid assemblyId, Guid userId, bool isVotingMember = true)
    {
        if (assemblyId == Guid.Empty)
            throw new DomainException("La asamblea es requerida");

        if (userId == Guid.Empty)
            throw new DomainException("El usuario es requerido");

        AssemblyId = assemblyId;
        UserId = userId;
        IsVotingMember = isVotingMember;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    public void GrantAdministratorRole()
    {
        if (!IsActive)
            throw new DomainException("No se puede otorgar rol de administrador a un participante inactivo");

        IsAdministrator = true;
    }

    public void RevokeAdministratorRole()
    {
        IsAdministrator = false;
    }

    public void AllowStartAssemblyVote()
    {
        if (!IsActive)
            throw new DomainException("No se puede otorgar permiso de voto a un participante inactivo");

        if (!IsVotingMember)
            throw new DomainException("El participante debe ser miembro votante");

        CanVoteToStartAssembly = true;
    }

    public void Deactivate()
    {
        IsActive = false;
        IsAdministrator = false;
        CanVoteToStartAssembly = false;
    }

    public void Activate()
    {
        IsActive = true;
    }
}
