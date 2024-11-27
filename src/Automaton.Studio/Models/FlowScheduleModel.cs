using Automaton.Studio.Enums;
using Automaton.Studio.Resources;
using CronExpressionDescriptor;
using Cronos;

namespace Automaton.Studio.Models;

public class FlowScheduleModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid FlowId { get; set; }
    public IEnumerable<Guid> RunnerIds { get; set; } = new List<Guid>();
    public CronRecurrence CronRecurrence { get; set; } = new CronRecurrence();
    public DateTime CreatedAt { get; set; }
    public bool IsNew { get; set; }
    public bool Loading { get; set; }

    public string CronNextExecution
    {
        get
        {
            if (CronRecurrence.CronType == CronType.Never)
                return Labels.CronNextExecutionNever;

            var expression = CronExpression.Parse(GetCron());
            var nextOccurence = expression.GetNextOccurrence(DateTime.UtcNow);

            return nextOccurence.ToString();
        }
    }

    public string CronDescription
    {
        get
        {
            if (CronRecurrence.CronType == CronType.Never)
                return Labels.CronDescriptionNever;

            var description = ExpressionDescriptor.GetDescription(GetCron(), new Options()
            {
                DayOfWeekStartIndexZero = true,
                Use24HourTimeFormat = true,
                Locale = "en"
            });

            return description;
        }
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
                return Hangfire.Cron.Weekly(dayOfWeek: CronRecurrence.DayOfWeek, hour: CronRecurrence.Hour, minute: CronRecurrence.Minute);
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
