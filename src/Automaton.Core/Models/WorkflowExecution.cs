using Automaton.Core.Enums;

namespace Automaton.Core.Models;

public class WorkflowExecution : IDisposable
{
    private bool hasErrors;
    private WorkflowStatus workflowStatus;

    public Guid Id { get; set; }
    public Guid FlowId { get; set; }
    public DateTime Started { get; set; }
    public DateTime Finished { get; set; }
    public string Status
    {
        get { return workflowStatus.ToString(); }
    }

    public WorkflowExecution(Guid flowId)
    {
        Id = Guid.NewGuid();
        FlowId = flowId;
        Started = DateTime.UtcNow;
        workflowStatus = WorkflowStatus.Working;
    }

    public void HasErrors()
    {
        hasErrors = true;
    }

    public void Dispose()
    {
        Finished = DateTime.UtcNow;
        workflowStatus = hasErrors ? WorkflowStatus.Error : WorkflowStatus.Success;
    }
}
