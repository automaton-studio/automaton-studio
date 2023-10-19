namespace Automaton.Studio.Models;

public class FlowScheduleModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid FlowId { get; set; }
    public IEnumerable<Guid> RunnerIds { get; set; } = new List<Guid>();
    public CronRecurrence CronRecurrence { get; set; } = new CronRecurrence();
    public string Cron { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsNew { get; set; }
    public bool Loading { get; set; }
}
