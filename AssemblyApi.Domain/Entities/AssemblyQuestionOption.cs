using AssemblyApi.Domain.Common;

namespace AssemblyApi.Domain.Entities;

public class AssemblyQuestionOption : Entity
{
    public Guid QuestionId { get; private set; }
    public string Text { get; private set; } = string.Empty;
    public string? Value { get; private set; }
    public int OrderIndex { get; private set; }
    public bool IsActive { get; private set; }

    private AssemblyQuestionOption() { }

    public AssemblyQuestionOption(Guid questionId, string text, int orderIndex, string? value = null)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentException("El texto no puede estar vacío");

        QuestionId = questionId;
        Text = text;
        OrderIndex = orderIndex;
        Value = value;
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void Activate()
    {
        IsActive = true;
    }
}
