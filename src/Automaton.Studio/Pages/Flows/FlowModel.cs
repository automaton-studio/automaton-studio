using Automaton.Core.Enums;

namespace Automaton.Studio.Pages.Flows;

public class FlowModel
{
    private readonly Dictionary<string, FlowStatusIcon> StatusIcons = new()
    {
        { WorkflowStatus.None.ToString(), new FlowStatusIcon { Icon = "check-circle", Class = "status-not-executed" } },
        { WorkflowStatus.Working.ToString(), new FlowStatusIcon { Icon = "eye", Class = "status-working" } },
        { WorkflowStatus.Success.ToString(), new FlowStatusIcon { Icon = "check-circle", Class = "status-success" } },
        { WorkflowStatus.Error.ToString(), new FlowStatusIcon { Icon = "exclamation-circle", Class = "status-error" } },
    };

    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
    public DateTime Started { get; set; }
    public DateTime Finished { get; set; }
    public string Status { get; set; } = WorkflowStatus.None.ToString();
    public IEnumerable<Guid> RunnerIds = new List<Guid>();

    public FlowStatusIcon StatusIcon
    {
        get
        {
            return StatusIcons.ContainsKey(Status) ? 
                StatusIcons[Status] : 
                StatusIcons[WorkflowStatus.None.ToString()];
        }
    }

    public bool WasExecuted()
    {
        return Status != WorkflowStatus.None.ToString();
    }
}
