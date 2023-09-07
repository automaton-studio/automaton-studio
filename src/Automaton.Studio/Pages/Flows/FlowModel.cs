using Automaton.Core.Enums;

namespace Automaton.Studio.Pages.Flows;

public class FlowModel
{
    private readonly Dictionary<WorkflowStatus, FlowStatusIcon> StatusIcons = new()
    {
        { WorkflowStatus.None, new FlowStatusIcon { Icon = "check-circle", Class = "status-none" } },
        { WorkflowStatus.Working, new FlowStatusIcon { Icon = "eye", Class = "status-working" } },
        { WorkflowStatus.Success, new FlowStatusIcon { Icon = "check-circle", Class = "status-success" } },
        { WorkflowStatus.Error, new FlowStatusIcon { Icon = "exclamation-circle", Class = "status-error" } },
    };

    private readonly Dictionary<WorkflowStatus, string> StatusMessages = new()
    {
        { WorkflowStatus.None, "-" },
        { WorkflowStatus.Working, "Working" },
        { WorkflowStatus.Success, "Success" },
        { WorkflowStatus.Error, "Error" },
    };

    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
    public DateTime Started { get; set; }
    public DateTime Finished { get; set; }
    public WorkflowStatus Status { get; set; } = WorkflowStatus.None;
    public IEnumerable<Guid> RunnerIds = new List<Guid>();

    public FlowStatusIcon StatusIcon => StatusIcons[Status];

    public string StatusMessage => StatusMessages[Status];

    public string StartedMessage
    {
        get
        {
            return Status != WorkflowStatus.None ? Started.ToString() : "-";
        }
    }

    public bool Running { get; set; }

    public bool HasRunners => RunnerIds != null && RunnerIds.Any();
    public bool DoesNotHaveRunners => !HasRunners;

    public void IsRunning() => Running = true;
    public void IsNotRunning() => Running = false;

}
