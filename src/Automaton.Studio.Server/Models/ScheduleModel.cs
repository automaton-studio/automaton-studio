namespace Automaton.Studio.Server.Models;

public class ScheduleModel
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public Guid FlowId { get; set; }

    public IEnumerable<Guid> RunnerIds { get; set; }

    public CronReccurence CronReccurence { get; set; } = CronReccurence.Never;

    public CronDate CronDate { get; set; } = new CronDate();

    public string? Cron { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string GetCron()
    {
        switch (CronReccurence)
        {
            case CronReccurence.Minutely:
                return Hangfire.Cron.Minutely();
            case CronReccurence.Hourly:
                return Hangfire.Cron.Hourly(CronDate.Minute);
            case CronReccurence.Daily:
                return Hangfire.Cron.Daily(hour: CronDate.Hour, minute: CronDate.Minute);
            case CronReccurence.Weekly:
                return Hangfire.Cron.Weekly(dayOfWeek: CronDate.DayOfWeek, hour: CronDate.Hour, minute: CronDate.Minute);
            case CronReccurence.Monthly:
                return Hangfire.Cron.Monthly(day: CronDate.Day, hour: CronDate.Hour, minute: CronDate.Minute);
            case CronReccurence.Yearly:
                return Hangfire.Cron.Yearly(month: CronDate.Month, day: CronDate.Day, hour: CronDate.Hour, minute: CronDate.Minute);
            case CronReccurence.Never:
                return Hangfire.Cron.Never();
            default:
                return Hangfire.Cron.Never();
        }
    }
}
