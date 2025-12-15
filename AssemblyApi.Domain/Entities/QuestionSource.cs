namespace AssemblyApi.Domain.Entities;

public class QuestionSource
{
    public string Id { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }

    private QuestionSource() { }

    public QuestionSource(string id, string name, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("El identificador es requerido");

        if (id.Length > 10)
            throw new ArgumentException("El identificador no puede superar 10 caracteres");

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre es requerido");

        Id = id;
        Name = name;
        Description = description;
    }
}
