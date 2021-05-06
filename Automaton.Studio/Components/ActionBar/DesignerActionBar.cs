using System;

namespace Automaton.Studio.Components.ActionBar
{
    public class DesignerActionBar : ActionBarComponent
    {
        public override Type GetViewComponent()
        {
            return typeof(DesignerActionBarComponent);
        }
    }
}
