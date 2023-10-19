namespace Automaton.Studio.Server.Models;

public class CronRecurrence
{
    public CronType CronType { get; set; } = CronType.Never;
    public int Minute { get; set; }
    public int Hour { get; set; }
    public int Day { get; set; }
    public DayOfWeek Week { get; set; }
    public int Month { get; set; }
}

public enum CronType
{
    Never,
    Minutely,
    Hourly,
    Daily,
    Weekly,
    Monthly,
    Yearly
}