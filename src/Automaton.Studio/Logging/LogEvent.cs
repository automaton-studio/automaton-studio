public class LogEvent
{
    public DateTime Timestamp { get; set; }

    public string Level { get; set; }

    public string MessageTemplate { get; set; }

    public string RenderedMessage { get; set; }

    public string? Exception { get; set; }

    public IDictionary<string, object> Properties { get; set; }
}