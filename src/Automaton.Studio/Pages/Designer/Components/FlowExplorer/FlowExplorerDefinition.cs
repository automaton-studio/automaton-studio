namespace Automaton.Studio.Pages.Designer.Components.FlowExplorer;

public class FlowExplorerDefinition
{
    private const string DefaultIcon = "file";
    private const string DefaultWorkflow = "Workflow";

    public string Id { get; set; }
    public string Name { get; set; } = DefaultWorkflow;
    public bool IsStartup { get; set; }
    public string Icon { get; set; } = DefaultIcon;
}
