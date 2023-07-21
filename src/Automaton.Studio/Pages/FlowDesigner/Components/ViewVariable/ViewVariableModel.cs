using Automaton.Core.Models;
using Automaton.Studio.Domain;

namespace Automaton.Studio.Pages.FlowDesigner.Components.ViewVariable;

public class ViewVariableModel
{
    public StepVariable Variable { get; set; }
    public StudioFlow Flow { get; set; }
}
