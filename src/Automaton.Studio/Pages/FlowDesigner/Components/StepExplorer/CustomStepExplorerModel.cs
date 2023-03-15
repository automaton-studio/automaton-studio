using Automaton.Studio.Domain;

namespace Automaton.Studio.Pages.FlowDesigner.Components.StepExplorer;

public class CustomStepExplorerModel : StepExplorerModel
{
    public CustomStepDefinition Definition { get; set; } = new CustomStepDefinition();
}
