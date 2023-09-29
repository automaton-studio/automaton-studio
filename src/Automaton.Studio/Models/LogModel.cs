using Automaton.Core.Enums;
using Newtonsoft.Json;

namespace Automaton.Studio.Models;

public class LogModel
{
    public int Id { get; set; }

    public string Level { get; set; }

    public string Message { get; set; }

    public string MessageTemplate { get; set; }

    public string Exception { get; set; }

    public string Properties { get; set; }

    public DateTime Timestamp { get; set; }

    public string EventType { get; set; }

    public string UserName { get; set; }

    public Dictionary<string, object> PropertiesDictionary
    {
        get { return JsonConvert.DeserializeObject<Dictionary<string, object>>(Properties); }
    }

    public string LevelClass => LevelClasses[Level];

    private readonly Dictionary<string, string> LevelClasses = new()
    {
        { "0", "default" },
        { "1", "default" },
        { "2", "success" },
        { "3", "warning" },
        { "4", "error" },
        { "5", "error" },
    };
}
