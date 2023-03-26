namespace Automaton.Core.Models;

public class Flow
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string StartupDefinitionId { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
    public IDictionary<string, object> Variables { get; set; }
    public IDictionary<string, object> InputVariables { get; set; }
    public IDictionary<string, object> OutputVariables { get; set; }
    public List<Definition> Definitions { get; set; }

    public Flow()
    {
        var defaultDefinition = new Definition();
        StartupDefinitionId = defaultDefinition.Id;
        Definitions = new List<Definition> { defaultDefinition };
        Variables = new Dictionary<string, object>();
        InputVariables = new Dictionary<string, object>();
        OutputVariables = new Dictionary<string, object>();
    }
}
