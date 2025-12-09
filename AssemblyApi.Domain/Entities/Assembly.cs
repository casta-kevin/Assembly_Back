using AssemblyApi.Domain.Common;
using AssemblyApi.Domain.Exceptions;

namespace AssemblyApi.Domain.Entities;

public class Assembly : Entity
{
    private readonly List<AssemblyParticipant> _participants = new();
    private readonly List<AssemblyQuestion> _questions = new();

    public Guid PropertyId { get; private set; }
    public Guid AssemblyStatusId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public string? Rules { get; private set; }
    public DateTime? StartDatePlanned { get; private set; }
    public DateTime? EndDatePlanned { get; private set; }
    public DateTime? StartDateActual { get; private set; }
    public DateTime? EndDateActual { get; private set; }
    public Guid CreatedByUserId { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public IReadOnlyCollection<AssemblyParticipant> Participants => _participants.AsReadOnly();
    public IReadOnlyCollection<AssemblyQuestion> Questions => _questions.AsReadOnly();

    private Assembly() { }

    public Assembly(Guid propertyId, Guid assemblyStatusId, string title, Guid createdByUserId)
    {
        if (propertyId == Guid.Empty)
            throw new DomainException("La propiedad es requerida");

        if (assemblyStatusId == Guid.Empty)
            throw new DomainException("El estado es requerido");

        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("El título es requerido");

        if (createdByUserId == Guid.Empty)
            throw new DomainException("El usuario creador es requerido");

        PropertyId = propertyId;
        AssemblyStatusId = assemblyStatusId;
        Title = title;
        CreatedByUserId = createdByUserId;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateDetails(string title, string? description, string? rules)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("El título no puede estar vacío");

        Title = title;
        Description = description;
        Rules = rules;
    }

    public void ScheduleDates(DateTime startDate, DateTime endDate)
    {
        if (endDate < startDate)
            throw new DomainException("La fecha de fin debe ser posterior a la fecha de inicio");

        if (startDate < DateTime.UtcNow)
            throw new DomainException("La fecha de inicio no puede ser en el pasado");

        StartDatePlanned = startDate;
        EndDatePlanned = endDate;
    }

    public void Start()
    {
        if (StartDateActual.HasValue)
            throw new InvalidAssemblyStateException("La asamblea ya ha sido iniciada");

        if (!_participants.Any())
            throw new InvalidAssemblyStateException("No se puede iniciar una asamblea sin participantes");

        StartDateActual = DateTime.UtcNow;
    }

    public void Close()
    {
        if (!StartDateActual.HasValue)
            throw new InvalidAssemblyStateException("La asamblea debe estar iniciada antes de cerrarla");

        if (EndDateActual.HasValue)
            throw new InvalidAssemblyStateException("La asamblea ya ha sido cerrada");

        EndDateActual = DateTime.UtcNow;
    }

    public void ChangeStatus(Guid newStatusId)
    {
        if (newStatusId == Guid.Empty)
            throw new DomainException("El estado es requerido");

        AssemblyStatusId = newStatusId;
    }

    public void AddParticipant(Guid userId, bool isVotingMember = true)
    {
        if (userId == Guid.Empty)
            throw new DomainException("El usuario es requerido");

        if (_participants.Any(p => p.UserId == userId))
            throw new DomainException("El participante ya está registrado");

        if (StartDateActual.HasValue)
            throw new InvalidAssemblyStateException("No se pueden agregar participantes a una asamblea iniciada");

        var participant = new AssemblyParticipant(Id, userId, isVotingMember);
        _participants.Add(participant);
    }

    public void RemoveParticipant(Guid userId)
    {
        if (StartDateActual.HasValue)
            throw new InvalidAssemblyStateException("No se pueden remover participantes de una asamblea iniciada");

        var participant = _participants.FirstOrDefault(p => p.UserId == userId);
        if (participant == null)
            throw new DomainException("El participante no existe");

        _participants.Remove(participant);
    }

    public void AddQuestion(string title, int orderIndex, Guid createdByUserId, Guid questionStatusId)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("El título de la pregunta es requerido");

        if (orderIndex < 0)
            throw new DomainException("El orden debe ser mayor o igual a cero");

        if (_questions.Any(q => q.OrderIndex == orderIndex))
            throw new DomainException("Ya existe una pregunta con ese orden");

        var question = new AssemblyQuestion(Id, title, orderIndex, createdByUserId, questionStatusId);
        _questions.Add(question);
    }

    public bool IsInProgress() => StartDateActual.HasValue && !EndDateActual.HasValue;
    
    public bool IsClosed() => EndDateActual.HasValue;
    
    public bool IsScheduled() => !StartDateActual.HasValue;
}
