using Automaton.Core.Enums;
using Automaton.Studio.Pages.Flows;

namespace Automaton.Studio.Models;

public class RunnerModel
{
    private readonly Dictionary<RunnerStatus, FlowStatusIcon> StatusIcons = new()
    {
        { RunnerStatus.None, new FlowStatusIcon { Icon = "eye", Class = "status-checking" } },
        { RunnerStatus.Online, new FlowStatusIcon { Icon = "check-circle", Class = "status-online" } },
        { RunnerStatus.Offline, new FlowStatusIcon { Icon = "exclamation-circle", Class = "status-offline" } },
    };

    public Guid Id { get; set; }
    public string Name { get; set; }
    public string UserId { get; set; }
    public string ConnectionId { get; set; }

    public RunnerStatus Status { get; set; } = RunnerStatus.None;

    public FlowStatusIcon StatusIcon
    {
        get
        {
            return StatusIcons.ContainsKey(Status) ?
                StatusIcons[Status] :
                StatusIcons[RunnerStatus.None];
        }
    }
}
