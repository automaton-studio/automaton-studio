using Automaton.Core.Enums;

namespace Automaton.Studio.Pages.CustomStepDesigner;

public class InputVariableModel
{
    public string Name { get; set; }
    public VariableType Type { get; set; }
    public string Description { get; set; }
    public IEnumerable<VariableType> Types { get; } = Enum.GetValues<VariableType>();
}
