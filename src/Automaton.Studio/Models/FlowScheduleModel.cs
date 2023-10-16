namespace Automaton.Studio.Models;

public class FlowScheduleModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid FlowId { get; set; }
    public IEnumerable<Guid> RunnerIds { get; set; } = new List<Guid>();
    public CronReccurence CronReccurence { get; set; } = CronReccurence.Never;
    public CronDate CronDate { get; set; } = new CronDate();
    public string Cron { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsNew { get; set; }
    public bool Loading { get; set; }
}
