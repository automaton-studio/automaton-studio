using Automaton.Core.Enums;

namespace Automaton.Core.Models;

public class WorkflowExecution
{
    private bool hasErrors;

    public Guid Id { get; set; }
    public Guid FlowId { get; set; }
    public DateTime Started { get; set; }
    public DateTime Finished { get; set; }
    public WorkflowStatus Status { get; set; }

    public void Start(Guid flowId)
    {
        Id = Guid.NewGuid();
        FlowId = flowId;
        Started = DateTime.UtcNow;
        Status = WorkflowStatus.Working;
    }

    public void Finish()
    {
        Finished = DateTime.UtcNow;
        Status = hasErrors ? WorkflowStatus.Error : WorkflowStatus.Success;
    }

    public void Error()
    {
        hasErrors = true;
    }
}
