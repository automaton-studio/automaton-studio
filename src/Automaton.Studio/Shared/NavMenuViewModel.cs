namespace Automaton.Studio.Shared;

public class NavMenuViewModel
{
    public const string IdParam = "Id";
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

    public MenuMetadata GetFlowMenu(Guid flowId, bool collapsed)
    {
        var flowMenu = menus[nameof(FlowMenu)];
        flowMenu.MenuParameters[IdParam] = flowId;
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
