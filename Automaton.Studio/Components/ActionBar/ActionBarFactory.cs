namespace Automaton.Studio.Components.ActionBar
{
    public static class ActionBarFactory
    {
        public static ActionBar GetActionBar(string routeName)
        {
            return routeName switch
            {
                "" or "dashboard" => new DashboardActionBar(),
                "designer" => new DesignerActionBar(),
                "workflows" => new WorkflowsActionBar(),
                _ => new DashboardActionBar(),
            };
        }
    }
}
