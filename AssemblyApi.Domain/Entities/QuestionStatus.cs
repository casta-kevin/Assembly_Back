using AssemblyApi.Domain.Common;

namespace AssemblyApi.Domain.Entities;

public class QuestionStatus : Entity
{
    public string Code { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }

    private QuestionStatus() { }

    public QuestionStatus(string code, string name, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("El código no puede estar vacío");

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre no puede estar vacío");

        Code = code;
        Name = name;
        Description = description;
    }
}
