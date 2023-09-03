using Automaton.Core.Enums;

namespace Automaton.Studio.Server.Models;

public class RunnerDetails
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? ConnectionId { get; set; }
    public RunnerStatus Status { get; set; }
}
