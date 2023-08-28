using Automaton.Core.Enums;

namespace Automaton.Core.Models;

public class WorkflowExecution
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

    public void Start(Guid flowId)
    {
        Id = Guid.NewGuid();
        FlowId = flowId;
        Started = DateTime.UtcNow;
        workflowStatus = WorkflowStatus.Working;
    }

    public void Finish()
    {
        Finished = DateTime.UtcNow;
        workflowStatus = hasErrors ? WorkflowStatus.Error : WorkflowStatus.Success;
    }

    public void Error()
    {
        hasErrors = true;
    }
}
