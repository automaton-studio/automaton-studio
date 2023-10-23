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
    public string Cron { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsNew { get; set; }
    public bool Loading { get; set; }

    public string CronNextExecution
    {
        get
        {
            if (CronRecurrence.CronType == CronType.Never)
                return Labels.CronNextExecutionNever;

            var expression = CronExpression.Parse(Cron);
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

            var description = ExpressionDescriptor.GetDescription(Cron, new Options()
            {
                DayOfWeekStartIndexZero = true,
                Use24HourTimeFormat = true,
                Locale = "en"
            });

            return description;
        }
    }
}
