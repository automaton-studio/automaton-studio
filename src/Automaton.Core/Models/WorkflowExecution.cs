using Automaton.Core.Enums;

namespace Automaton.Core.Models;

public class WorkflowExecution : IDisposable
{
    private bool hasErrors;

    public Guid Id { get; set; }
    public Guid FlowId { get; set; }
    public DateTime Started { get; set; }
    public DateTime Finished { get; set; }
    public WorkflowStatus Status { get; set; }

    public WorkflowExecution(Guid flowId)
    {
        Id = Guid.NewGuid();
        FlowId = flowId;
        Started = DateTime.UtcNow;
        Status = WorkflowStatus.Working;
    }

    public void HasErrors()
    {
        hasErrors = true;
    }

    public void Dispose()
    {
        Finished = DateTime.UtcNow;
        Status = hasErrors ? WorkflowStatus.Error : WorkflowStatus.Success;
    }
}
