using System;

namespace Automaton.Studio.Components.ActionBar
{
    public class DashboardActionBar : ActionBar
    {
        public override Type GetViewComponent()
        {
            return typeof(DashboardActionBarComponent);
        }
    }
}
