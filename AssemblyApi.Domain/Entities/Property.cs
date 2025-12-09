using AssemblyApi.Domain.Common;

namespace AssemblyApi.Domain.Entities;

public class Property : Entity
{
    public string Name { get; private set; } = string.Empty;

    private Property() { }

    public Property(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre no puede estar vacío");

        Name = name;
    }

    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre no puede estar vacío");

        Name = name;
    }
}
