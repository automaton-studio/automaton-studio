using Automaton.Core.Enums;

namespace Automaton.Core.Models;

public class StepVariable
{
    public string? Id { get; set; }
    public required string? Name { get; set; }
    public VariableType Type { get; set; } = VariableType.String;
    public object? Value { get; set; }
    public string? Description { get; set; }

    public Type GetRealType()
    {
        return Type switch
        {
            VariableType.String or VariableType.Text => typeof(string),
            VariableType.Boolean => typeof(bool),
            VariableType.Number => typeof(decimal),
            VariableType.Date => typeof(DateTime),
            _ => typeof(string),
        };
    }
}
