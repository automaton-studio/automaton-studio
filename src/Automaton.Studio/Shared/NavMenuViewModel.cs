namespace Automaton.Studio.Shared;

public class NavMenuViewModel
{
    public const string FlowIdParam = "FlowId";
    public const string FlowNameParam = "FlowName";
    public const string CollapsedParam = "Collapsed";

    private readonly Dictionary<string, MenuMetadata> menus = new()
    {
        {
            nameof(MainMenu),
            new MenuMetadata { MenuType = typeof(MainMenu) }
        },
        {
            nameof(FlowMenu),
            new MenuMetadata { MenuType = typeof(FlowMenu) }
        }
    };

    public MenuMetadata GetFlowMenu(Guid flowId, string flowName, bool collapsed)
    {
        var flowMenu = menus[nameof(FlowMenu)];
        flowMenu.MenuParameters[FlowIdParam] = flowId;
        flowMenu.MenuParameters[FlowNameParam] = flowName;
        flowMenu.MenuParameters[CollapsedParam] = collapsed;

        return flowMenu;
    }

    public MenuMetadata GetMainMenu(bool collapsed)
    {
        var flowMenu = menus[nameof(MainMenu)];
        flowMenu.MenuParameters[CollapsedParam] = collapsed;

        return flowMenu;
    }
}
