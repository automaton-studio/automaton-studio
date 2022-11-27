namespace Automaton.Core.Models;

public class StepVariable
{
    public string? OldName { get; set; }
    public string? Name { get; set; }
    public object? Value { get; set; }

    public bool IsNew()
    {
        return string.IsNullOrEmpty(OldName);
    }
}
