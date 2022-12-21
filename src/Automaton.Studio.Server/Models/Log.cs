#nullable disable

namespace Automaton.Studio.Server.Models;

public partial class Log
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string LogLevel { get; set; }
    public string EventName { get; set; }
    public string Source { get; set; }
    public string ExceptionMessage { get; set; }
    public string StackTrace { get; set; }
    public DateTime CreatedDate { get; set; }
}
