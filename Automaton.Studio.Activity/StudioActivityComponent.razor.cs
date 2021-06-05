using Microsoft.AspNetCore.Components;

namespace Automaton.Studio.Activity
{
    public partial class StudioActivityComponent : ComponentBase
    {
        /// <summary>
        /// Specifies one or more classnames for the Dropzone element.
        /// </summary>
        [Parameter]
        public string? Class { get; set; }

        protected override void OnInitialized()
        {
            Class = "designer-activity";

            base.OnInitialized();
        }
    }
}
