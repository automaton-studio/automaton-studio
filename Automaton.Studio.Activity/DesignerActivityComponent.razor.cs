using AntDesign;
using Automaton.Studio.Core.Extensions;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Automaton.Studio.Core
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

        /// <summary>
        /// AntDesign modal service
        /// </summary>
        [Inject] 
        private ModalService ModalService { get; set; } = default!;

        protected override void OnInitialized()
        {
            base.OnInitialized();
        }

        #region Event Handlers

        private async Task OnEdit(StudioActivity activity)
        {
            var result = await activity.EditActivityDialog(ModalService);

            result.OnOk = () => {

                StateHasChanged();

                return Task.CompletedTask;
            };
        }

        private static void OnDelete(StudioActivity activity)
        {
            activity.StudioWorkflow.RemoveActivity(activity);
        }

        #endregion
    }
}
