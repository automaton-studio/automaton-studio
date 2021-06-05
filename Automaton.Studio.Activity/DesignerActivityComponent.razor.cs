using Microsoft.AspNetCore.Components;

namespace Automaton.Studio.Activity
{
    public partial class DesignerActivityComponent : ComponentBase
    {
        /// <summary>
        /// Associated studio activity
        /// </summary>
        [Parameter]
        public StudioActivity Activity { get; set; }

        /// <summary>
        /// Child content
        /// </summary>
        [Parameter] 
        public RenderFragment ChildContent { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
        }
    }
}
