using System;

namespace Automaton.Studio.Components.ActionBar
{
    public class WorkflowsActionBar : ActionBarComponent
    {
        public override Type GetViewComponent()
        {
            return typeof(WorkflowsActionBarComponent);
        }
    }
}
