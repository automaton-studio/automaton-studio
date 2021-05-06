using Automaton.Studio.Enums;
using System;

namespace Automaton.Studio.Components.ActionBar
{
    public static class ActionBarFactory
    {
        public static ActionBar GetActionBar(StudioNavigation navigation)
        {
            switch(navigation)
            {
                case StudioNavigation.Dashboard:
                    return new DashboardActionBar();

                case StudioNavigation.Designer:
                    return new DesignerActionBar();

                case StudioNavigation.Workflows:
                    return new WorkflowsActionBar();

                default:
                    throw new NotImplementedException(navigation.ToString());
            }
        }
    }
}
