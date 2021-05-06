using System;

namespace Automaton.Studio.Components.ActionBar
{
    public class DashboardActionBar : ActionBarComponent
    {
        public override Type GetViewComponent()
        {
            return typeof(DashboardActionBarComponent);
        }
    }
}
