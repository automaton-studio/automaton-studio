namespace Automaton.Core.Models;

public class Flow
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string StartupDefinitionId { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
    public IDictionary<string, object> Variables { get; set; } = new Dictionary<string, object>();
    public IDictionary<string, object> InputVariables { get; set; } = new Dictionary<string, object>();
    public IDictionary<string, object> OutputVariables { get; set; } = new Dictionary<string, object>();
    public List<Definition> Definitions { get; set; } = new List<Definition>();
}
