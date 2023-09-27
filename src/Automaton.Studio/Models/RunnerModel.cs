using Automaton.Core.Enums;

namespace Automaton.Studio.Models;

public class RunnerModel
{
    private readonly Dictionary<RunnerStatus, string> SrarusClasses = new()
    {
        { RunnerStatus.None, "badge badge-none" },
        { RunnerStatus.Online, "badge badge-success" },
        { RunnerStatus.Offline,"badge badge-error" },
    };

    public Guid Id { get; set; }
    public string Name { get; set; }
    public string UserId { get; set; }
    public string ConnectionId { get; set; }

    public RunnerStatus Status { get; set; } = RunnerStatus.None;

    public string StatusClass
    {
        get
        {
            return SrarusClasses.ContainsKey(Status) ?
                SrarusClasses[Status] :
                SrarusClasses[RunnerStatus.None];
        }
    }
}
