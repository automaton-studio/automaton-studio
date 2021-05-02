using System;

namespace Automaton.Studio.Components.ActionBar
{
    public class WorkflowsActionBar : ActionBar
    {
        public override Type GetViewComponent()
        {
            return typeof(WorkflowsActionBarComponent);
        }
    }
}
