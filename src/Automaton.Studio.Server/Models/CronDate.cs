namespace Automaton.Studio.Server.Models;

public class CronDate
{
    public int Minute { get; set; }
    public int Hour { get; set; }
    public int Day { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public int Week { get; set; }
    public int Month { get; set; }
}
