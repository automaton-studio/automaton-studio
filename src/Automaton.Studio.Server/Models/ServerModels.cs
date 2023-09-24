namespace Automaton.Studio.Server.Models
{
    public class ServerModels
    {
        public record FlowExecutionResult(IEnumerable<Entities.FlowExecution> FlowExecutions, int Total);
    }
}
