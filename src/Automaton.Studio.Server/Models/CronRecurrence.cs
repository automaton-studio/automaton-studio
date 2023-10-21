using Automaton.Studio.Server.Enums;

namespace Automaton.Studio.Server.Models;

public class CronRecurrence
{
    public CronType CronType { get; set; }
    public int Minute { get; set; }
    public int Hour { get; set; }
    public int Day { get; set; }
    public DayOfWeek Week { get; set; }
    public int Month { get; set; }

    public CronRecurrence()
    {
        CronType = CronType.Never;
    }

    public CronRecurrence(CronType cronType) : base()
    {
        CronType = cronType;
    }
}