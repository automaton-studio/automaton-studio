namespace Automaton.Core.Models;

public class Workflow
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string StartupDefinitionId { get; set; }

    public IDictionary<string, StepVariable> Variables { get; set; }

    public IDictionary<string, object> OutputVariables { get; set; }

    public IDictionary<string, object> InputVariables { get; set; }

    public List<WorkflowDefinition> Definitions { get; set; }

    public Workflow()
    {
        Variables = new Dictionary<string, StepVariable>();
        OutputVariables = new Dictionary<string, object>();
        InputVariables = new Dictionary<string, object>();
        Definitions = new List<WorkflowDefinition>();
    }

    public WorkflowDefinition GetStartupDefinition()
    {
        return Definitions.SingleOrDefault(x => x.Id == StartupDefinitionId);
    }

    public bool VariableExists(string key)
    {
        return Variables.ContainsKey(key);
    }

    public KeyValuePair<string, StepVariable> GetVariable(string key)
    {
        return new KeyValuePair<string, StepVariable>(key, Variables[key]);
    }

    public IEnumerable<KeyValuePair<string, StepVariable>> GetVariables(IEnumerable<string> names)
    {
        var variables = Variables.Where(x => names.Contains(x.Key)).Select(x => new KeyValuePair<string, StepVariable>(x.Key, x.Value));

        return variables;
    }

    public void SetVariable(StepVariable variable)
    {
        if (Variables.ContainsKey(variable.Name))
        {
            Variables[variable.Name] = variable;
        }
        else
        {
            Variables.Add(variable.Name, variable);
        }
    }
}
