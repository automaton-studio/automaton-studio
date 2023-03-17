namespace Automaton.Studio.Domain;

public class CustomStep
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string MoreInfo { get; set; } = string.Empty;
    public bool VisibleInExplorer { get; set; }
    public string Icon { get; set; } = string.Empty;
    public CustomStepDefinition Definition { get; set; } = new CustomStepDefinition();
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
}
