using Automaton.Studio.Enums;
using System;

namespace Automaton.Studio.Components.ActionBar
{
    public static class ActionBarFactory
    {
        public static ActionBarComponent GetActionBar(Enums.ActionBar actionBar)
        {
            switch(actionBar)
            {
                case Enums.ActionBar.Dashboard:
                    return new DashboardActionBar();

                case Enums.ActionBar.Designer:
                    return new DesignerActionBar();

                case Enums.ActionBar.Workflows:
                    return new WorkflowsActionBar();

                default:
                    throw new NotImplementedException("Invalid ActionBar");
            }
        }
    }
}
