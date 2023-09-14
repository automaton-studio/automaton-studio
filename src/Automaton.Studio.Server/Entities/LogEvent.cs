using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Automaton.Studio.Server.Entities;

public class LogEvent
{
    public int Id { get; set; }

    [Column("_ts")]
    public DateTime Timestamp { get; set; }

    public string Level { get; set; }

    public string Template { get; set; }

    public string Message { get; set; }

    public string? Exception { get; set; }

    public string Properties { get; set; }

    [NotMapped]
    public Dictionary<string, object>? PropertiesDictionary
    {
        get { return JsonConvert.DeserializeObject<Dictionary<string, object>>(Properties); }
    }
}
