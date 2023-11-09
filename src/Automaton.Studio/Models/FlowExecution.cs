#nullable disable

using Automaton.Core.Enums;
using Automaton.Studio.Domain;

namespace Automaton.Studio.Models;

public class FlowExecution
{
    private readonly Dictionary<WorkflowStatus, string> StatusClasses = new()
    {
        { WorkflowStatus.None, "badge badge-none" },
        { WorkflowStatus.Working, "badge badge-working" },
        { WorkflowStatus.Success, "badge badge-success" },
        { WorkflowStatus.Error, "badge badge-error" },
    };

    public Guid Id { get; set; }
    public Guid FlowId { get; set; }
    public DateTime Started { get; set; }
    public DateTime Finished { get; set; }
    public WorkflowStatus Status { get; set; }
    public string Application { get; set; }

    public string LogsText { get; set; } = string.Empty;

    public string StatusClass => StatusClasses[Status];
}

