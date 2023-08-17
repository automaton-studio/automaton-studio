using Automaton.Core.Extensions;
using Serilog.Core;
using Serilog.Events;

namespace Automaton.Core.Logs;

/// <summary>
/// Serilog sink that capture logs created at workflow execution and stores them in Logs property
/// </summary>
public class WorkflowLogsSink : ILogEventSink
{
    private readonly IList<LogEvent> logEvents = new List<LogEvent>();

    public string Logs => string.Join(Environment.NewLine, logEvents.Select(x => $"{x.Timestamp:s} {x.RenderMessage()}"));

    public void Emit(LogEvent logEvent)
    {
        if (logEvent == null) 
            throw new ArgumentNullException(nameof(logEvent));

        if (logEvent.IsWorkflowExecution())
            logEvents.Add(logEvent);
    }

    public void ClearLogs()
    {
        logEvents.Clear();
    }
}
