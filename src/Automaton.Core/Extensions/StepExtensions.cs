using Automaton.Core.Models;

namespace Automaton.Studio.Extensions;

public static class StepExtensions
{
    public static Type FindType(this Step step)
    {
        var fullClassName = $"Automaton.Steps.{step.Type}, Automaton.Steps";
        var type = Type.GetType(fullClassName, true, true);

        return type;
    }
}
