using Automaton.Studio.Domain;

namespace Automaton.Studio.Pages.CustomStepDesigner;

public class InputVariableModel
{
    public string Name { get; set; }
    public string Type { get; set; }
    public string Description { get; set; }
    public IEnumerable<string> Types { get; } = Enum.GetNames(typeof(VariableType));
}
