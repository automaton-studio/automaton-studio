namespace Automaton.Core.Models;

public class StepVariable
{
    public string? Name { get; set; }
    public string? Type { get; set; }
    public object? Value { get; set; }
    public string? Description { get; set; }

    public StepVariable()
    {
    }

    public StepVariable(string name)
    {
        Name = name;
    }

    public StepVariable(string name, object value)
    {
        Name = name;
        Value = value;
    }
}
