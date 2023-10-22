using Automaton.Studio.Enums;

namespace Automaton.Studio.Models;

public class CronRecurrence
{
    public CronType CronType { get; set; }
    public int Minute { get; set; }
    public int Hour { get; set; }
    public int Day { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public int Month { get; set; }

    public CronRecurrence()
    {
        CronType = CronType.Never;
        Hour = 0;
        Day = 1;
        Month = 1;
        DayOfWeek = DayOfWeek.Monday;
    }
}

