namespace Automaton.Studio.Models;

public record FlowExecutionResult(IEnumerable<FlowExecution> FlowExecutions, int Total);

public record FlowLogsResult(IEnumerable<LogModel> Logs, int Total);

