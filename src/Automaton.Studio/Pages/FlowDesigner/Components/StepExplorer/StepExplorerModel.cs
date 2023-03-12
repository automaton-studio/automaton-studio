namespace Automaton.Studio.Pages.FlowDesigner.Components.StepExplorer;

public class StepExplorerModel
{
    private const string DefaultIcon = "file";

    public string Name { get; set; }
    public string DisplayName { get; set; }
    public string Type { get; set; }
    public string Description { get; set; }
    public string Icon { get; set; } = DefaultIcon;
    public string Category { get; set; }
    public bool VisibleInExplorer { get; set; }
    public bool IsCustomStep { get; set; }

    public IList<StepExplorerModel> Steps { get; set; } = new List<StepExplorerModel>();

    public StepExplorerModel()
    {
        Name = "Step";
        Type = "Step";
    }

    public StepExplorerModel(string name, string displayName)
    {
        Name = name;
        DisplayName = displayName;
    }

    public void AddStep(StepExplorerModel step)
    {
        Steps.Add(step);  
    }

    public bool IsCategory()
    {
        return Steps != null && Steps.Any();
    }
}
