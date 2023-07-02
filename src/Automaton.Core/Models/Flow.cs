namespace Automaton.Core.Models;

public class Flow
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string StartupDefinitionId { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
    public IDictionary<string, StepVariable> Variables { get; set; } = new Dictionary<string, StepVariable>();
    public IDictionary<string, StepVariable> InputVariables { get; set; } = new Dictionary<string, StepVariable>();
    public IDictionary<string, StepVariable> OutputVariables { get; set; } = new Dictionary<string, StepVariable>();
    public List<Definition> Definitions { get; set; } = new List<Definition>();
}
