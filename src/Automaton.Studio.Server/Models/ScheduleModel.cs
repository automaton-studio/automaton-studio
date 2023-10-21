using Automaton.Studio.Server.Enums;

namespace Automaton.Studio.Server.Models;

public class ScheduleModel
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public Guid FlowId { get; set; }

    public IEnumerable<Guid> RunnerIds { get; set; }

    public CronRecurrence CronRecurrence { get; set; }

    public string? Cron { get; set; }

    public DateTime? CreatedAt { get; set; }

    public ScheduleModel()
    {
        CronRecurrence = new CronRecurrence();
    }

    public string GetCron()
    {
        switch (CronRecurrence.CronType)
        {
            case CronType.Minutely:
                return Hangfire.Cron.Minutely();
            case CronType.Hourly:
                return Hangfire.Cron.Hourly(CronRecurrence.Minute);
            case CronType.Daily:
                return Hangfire.Cron.Daily(hour: CronRecurrence.Hour, minute: CronRecurrence.Minute);
            case CronType.Weekly:
                return Hangfire.Cron.Weekly(dayOfWeek: CronRecurrence.Week, hour: CronRecurrence.Hour, minute: CronRecurrence.Minute);
            case CronType.Monthly:
                return Hangfire.Cron.Monthly(day: CronRecurrence.Day, hour: CronRecurrence.Hour, minute: CronRecurrence.Minute);
            case CronType.Yearly:
                return Hangfire.Cron.Yearly(month: CronRecurrence.Month, day: CronRecurrence.Day, hour: CronRecurrence.Hour, minute: CronRecurrence.Minute);
            case CronType.Never:
                return Hangfire.Cron.Never();
            default:
                return Hangfire.Cron.Never();
        }
    }
}
