namespace Automaton.Studio.Models;

public record FlowExecutionResult(IEnumerable<FlowExecution> FlowExecutions, int Total);

public record FlowLogsResult(IEnumerable<LogModel> Logs, int Total);

public class FlowScheduleResult
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid FlowId { get; set; }
    public IEnumerable<Guid> RunnerIds { get; set; }
}

