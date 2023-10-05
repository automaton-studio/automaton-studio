namespace Automaton.Studio.Server.Models;

public class ScheduleModel
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public Guid FlowId { get; set; }

    public IEnumerable<Guid> RunnerIds { get; set; }
}
