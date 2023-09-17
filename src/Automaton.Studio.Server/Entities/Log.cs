using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace Automaton.Studio.Server.Entities;

public class Log
{
    public int Id { get; set; }

    public string Level { get; set; }

    public string Message { get; set; }

    public string Template { get; set; }

    public string? Exception { get; set; }

    public string Properties { get; set; }

    public DateTime Timestamp { get; set; }

    [NotMapped]
    public Dictionary<string, object>? PropertiesDictionary
    {
        get { return JsonConvert.DeserializeObject<Dictionary<string, object>>(Properties); }
    }
}
