namespace Automaton.Core.Models;

public class WorkflowExecutorResult
{
    public List<ExecutionError> Errors { get; set; } = new List<ExecutionError>();
}
