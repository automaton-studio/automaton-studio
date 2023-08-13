using System.Text.Json.Serialization;

namespace Automaton.Core.Logs;

public class SerilogHttpLogEvent
{
    [JsonPropertyName("Timestamp")]
    public DateTime Timestamp { get; set; }

    [JsonPropertyName("Level")]
    public string Level { get; set; } = "Information";

    [JsonPropertyName("MessageTemplate")]
    public string? MessageTemplate { get; set; }

    [JsonPropertyName("RenderedMessage")]
    public string? RenderedMessage { get; set; }

    [JsonPropertyName("Properties")]
    public Dictionary<string, object>? Properties { get; set; }

    [JsonPropertyName("Exception")]
    public string? Exception { get; set; } 
}
