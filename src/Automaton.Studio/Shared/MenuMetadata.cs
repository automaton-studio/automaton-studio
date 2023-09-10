namespace Automaton.Studio.Shared;

public class MenuMetadata
{
    public Type MenuType { get; set; }
    public Dictionary<string, object> MenuParameters { get; set; } = new Dictionary<string, object>();

    public void SetCollapsed(bool collapsed)
    {
        MenuParameters[NavMenuViewModel.CollapsedParam] = collapsed;
    }
}
