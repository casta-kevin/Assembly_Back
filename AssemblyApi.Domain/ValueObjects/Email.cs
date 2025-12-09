namespace AssemblyApi.Domain.ValueObjects;

public record Email
{
    public string Value { get; }

    private Email(string value)
    {
        Value = value;
    }

    public static Email Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El email no puede estar vacío");

        if (!value.Contains('@'))
            throw new ArgumentException("El email debe tener un formato válido");

        return new Email(value);
    }
}
