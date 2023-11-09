namespace Automaton.Core.Models;

public class ExecutionResult
{
    public bool Proceed { get; set; }

    public static ExecutionResult Next()
    {
        return new ExecutionResult
        {
            Proceed = true
        };
    }

    public static ExecutionResult Stop()
    {
        return new ExecutionResult
        {
            Proceed = false
        };
    }
}
