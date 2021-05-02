using System;

namespace Automaton.Studio.Components.ActionBar
{
    public class DesignerActionBar : ActionBar
    {
        public override Type GetViewComponent()
        {
            return typeof(DesignerActionBarComponent);
        }
    }
}
