using AssemblyApi.Domain.Common;
using AssemblyApi.Domain.Exceptions;

namespace AssemblyApi.Domain.Entities;

public class AssemblyQuestion : Entity
{
    private readonly List<AssemblyQuestionOption> _options = new();

    public Guid AssemblyId { get; private set; }
    public string QuestionStatusId { get; private set; } = string.Empty;
    public string QuestionSourceId { get; private set; } = string.Empty;
    public string Title { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public DateTime? StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public int OrderIndex { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public Guid CreatedByUserId { get; private set; }

    public IReadOnlyCollection<AssemblyQuestionOption> Options => _options.AsReadOnly();

    private AssemblyQuestion() { }

    public AssemblyQuestion(
        Guid assemblyId,
        string title,
        Guid createdByUserId,
        string questionStatusId,
        string questionSourceId,
        int orderIndex)
    {
        if (assemblyId == Guid.Empty)
            throw new DomainException("La asamblea es requerida");

        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("El titulo es requerido");

        if (createdByUserId == Guid.Empty)
            throw new DomainException("El usuario creador es requerido");

        if (string.IsNullOrWhiteSpace(questionStatusId))
            throw new DomainException("El estado es requerido");

        if (string.IsNullOrWhiteSpace(questionSourceId))
            throw new DomainException("El origen de la pregunta es requerido");

        if (orderIndex <= 0)
            throw new DomainException("El orden debe ser mayor a cero");

        AssemblyId = assemblyId;
        Title = title;
        CreatedByUserId = createdByUserId;
        QuestionStatusId = questionStatusId;
        QuestionSourceId = questionSourceId;
        OrderIndex = orderIndex;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("El titulo no puede estar vacio");

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

    public void ChangeStatus(string newStatusId)
    {
        if (string.IsNullOrWhiteSpace(newStatusId))
            throw new DomainException("El estado es requerido");

        QuestionStatusId = newStatusId;
    }

    public void UpdateSource(string sourceId)
    {
        if (string.IsNullOrWhiteSpace(sourceId))
            throw new DomainException("El origen es requerido");

        QuestionSourceId = sourceId;
    }

    public void UpdateOrder(int orderIndex)
    {
        if (orderIndex <= 0)
            throw new DomainException("El orden debe ser mayor a cero");

        OrderIndex = orderIndex;
    }

    public AssemblyQuestionOption AddOption(string text, string? value = null)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new DomainException("El texto de la opcion es requerido");

        if (_options.Count >= 10)
            throw new DomainException("No se pueden agregar mas de 10 opciones");

        var option = new AssemblyQuestionOption(Id, text, value);
        _options.Add(option);
        return option;
    }

    public bool IsActive() => Options.Any(o => o.IsActive);
}
