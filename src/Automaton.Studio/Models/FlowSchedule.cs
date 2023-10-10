namespace Automaton.Studio.Models;

public class FlowSchedule
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid FlowId { get; set; }
    public IEnumerable<Guid> RunnerIds { get; set; }
}
