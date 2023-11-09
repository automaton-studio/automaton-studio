using Automaton.Core.Enums;
using Automaton.Studio.Domain;

namespace Automaton.Studio.Pages.Flows;

public class FlowModel
{
    private readonly Dictionary<WorkflowStatus, string> StatusClasses = new()
    {
        { WorkflowStatus.None, "badge badge-none" },
        { WorkflowStatus.Working, "badge badge-working" },
        { WorkflowStatus.Success, "badge badge-success" },
        { WorkflowStatus.Error, "badge badge-error" },
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

    public string StatusClass => StatusClasses[Status];

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
