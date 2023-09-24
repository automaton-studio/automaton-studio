namespace Automaton.Studio.Server.Models
{
    public class ApiModels
    {
        public record FlowExecutionResult(IEnumerable<Entities.FlowExecution> FlowExecutions, int Total);
    }
}
