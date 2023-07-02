using Automaton.Core.Enums;

namespace Automaton.Core.Models;

public class StepVariable
{
    public string? Id { get; set; }
    public required string? Name { get; set; }
    public VariableType Type { get; set; } = VariableType.String;
    public object? Value { get; set; }
    public string? Description { get; set; }
}
