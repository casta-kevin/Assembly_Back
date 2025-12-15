using AssemblyApi.Domain.Common;

namespace AssemblyApi.Domain.Entities;

public class AssemblyStatus : Entity
{
    public string Id { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }

    private AssemblyStatus() { }

    public AssemblyStatus(string id, string name, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("El identificador es requerido");

        if (id.Length > 10)
            throw new ArgumentException("El identificador no puede superar 10 caracteres");

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre no puede estar vacío");

        Id = id;
        Name = name;
        Description = description;
    }
}
