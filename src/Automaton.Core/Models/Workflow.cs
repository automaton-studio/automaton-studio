namespace Automaton.Core.Models;

public class Workflow
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string StartupDefinitionId { get; set; }

    public IDictionary<string, object> Variables { get; set; }

    public IDictionary<string, object> OutputVariables { get; set; }

    public List<WorkflowDefinition> Definitions { get; set; }

    public Workflow()
    {
        Variables = new Dictionary<string, object>();
        OutputVariables = new Dictionary<string, object>();
        Definitions = new List<WorkflowDefinition>();
    }

    public WorkflowDefinition GetStartupDefinition()
    {
        return Definitions.SingleOrDefault(x => x.Id == StartupDefinitionId);
    }

    public KeyValuePair<string, object> GetVariable(string key)
    {
        return new KeyValuePair<string, object>(key, Variables[key]);
    }

    public IEnumerable<KeyValuePair<string, object>> GetVariables(IEnumerable<string> names)
    {
        var variables = Variables.Where(x => names.Contains(x.Key)).Select(x => new KeyValuePair<string, object>(x.Key, x.Value));

        return variables;
    }
}
