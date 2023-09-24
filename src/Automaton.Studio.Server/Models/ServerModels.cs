namespace Automaton.Studio.Server.Models
{
    public record FlowExecutionResult(IEnumerable<Entities.FlowExecution> FlowExecutions, int Total);
}
