#nullable disable

using Automaton.Core.Enums;
using Automaton.Studio.Pages.Flows;

namespace Automaton.Studio.Models;

public class FlowExecution
{
    private readonly Dictionary<WorkflowStatus, FlowStatusIcon> StatusIcons = new()
    {
        { WorkflowStatus.None, new FlowStatusIcon { Icon = "check-circle", Class = "status-none" } },
        { WorkflowStatus.Working, new FlowStatusIcon { Icon = "eye", Class = "status-working" } },
        { WorkflowStatus.Success, new FlowStatusIcon { Icon = "check-circle", Class = "status-success" } },
        { WorkflowStatus.Error, new FlowStatusIcon { Icon = "exclamation-circle", Class = "status-error" } },
    };

    public Guid Id { get; set; }
    public Guid FlowId { get; set; }
    public DateTime Started { get; set; }
    public DateTime Finished { get; set; }
    public WorkflowStatus Status { get; set; }

    public FlowStatusIcon StatusIcon => StatusIcons[Status];
}

