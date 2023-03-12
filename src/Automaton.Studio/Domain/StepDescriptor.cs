namespace Automaton.Studio.Domain;

public class StepDescriptor
{
    public string Name { get; set; }
    public string Type { get; set; }
    public string DisplayName { get; set; }
    public string Description { get; set; }
    public string MoreInfo { get; set; }
    public string Category { get; set; }
    public string Icon { get; set; }
    public bool VisibleInExplorer { get; set; } = true;
}
