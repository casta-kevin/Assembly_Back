using AssemblyApi.Domain.Common;
using AssemblyApi.Domain.Exceptions;

namespace AssemblyApi.Domain.Entities;

public class AssemblyQuestion : Entity
{
    private readonly List<AssemblyQuestionOption> _options = new();

    public Guid AssemblyId { get; private set; }
    public Guid QuestionStatusId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public int OrderIndex { get; private set; }
    public DateTime? StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public Guid CreatedByUserId { get; private set; }

    public IReadOnlyCollection<AssemblyQuestionOption> Options => _options.AsReadOnly();

    private AssemblyQuestion() { }

    public AssemblyQuestion(Guid assemblyId, string title, int orderIndex, Guid createdByUserId, Guid questionStatusId)
    {
        if (assemblyId == Guid.Empty)
            throw new DomainException("La asamblea es requerida");

        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("El título es requerido");

        if (orderIndex < 0)
            throw new DomainException("El orden debe ser mayor o igual a cero");

        if (createdByUserId == Guid.Empty)
            throw new DomainException("El usuario creador es requerido");

        if (questionStatusId == Guid.Empty)
            throw new DomainException("El estado es requerido");

        AssemblyId = assemblyId;
        Title = title;
        OrderIndex = orderIndex;
        CreatedByUserId = createdByUserId;
        QuestionStatusId = questionStatusId;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("El título no puede estar vacío");

        Title = title;
    }

    public void UpdateDescription(string? description)
    {
        Description = description;
    }

    public void SetSchedule(DateTime startDate, DateTime endDate)
    {
        if (endDate < startDate)
            throw new DomainException("La fecha de fin debe ser posterior a la fecha de inicio");

        StartDate = startDate;
        EndDate = endDate;
    }

    public void ChangeStatus(Guid newStatusId)
    {
        if (newStatusId == Guid.Empty)
            throw new DomainException("El estado es requerido");

        QuestionStatusId = newStatusId;
    }

    public void AddOption(string text, int orderIndex, string? value = null)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new DomainException("El texto de la opción es requerido");

        if (orderIndex < 0)
            throw new DomainException("El orden debe ser mayor o igual a cero");

        if (_options.Any(o => o.OrderIndex == orderIndex))
            throw new DomainException("Ya existe una opción con ese orden");

        if (_options.Count >= 10)
            throw new DomainException("No se pueden agregar más de 10 opciones");

        var option = new AssemblyQuestionOption(Id, text, orderIndex, value);
        _options.Add(option);
    }

    public bool IsActive() => Options.Any(o => o.IsActive);
}
