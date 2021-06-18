using AntDesign;
using Automaton.Studio.Activity.Extensions;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

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

        /// <summary>
        /// AntDesign modal service
        /// </summary>
        [Inject] 
        ModalService ModalService { get; set; } = default!;

        protected override void OnInitialized()
        {
            base.OnInitialized();
        }

        #region Event Handlers

        private async Task OnEdit(StudioActivity activity)
        {
            await EditActivityDialog(activity);
        }

        private void OnDelete(StudioActivity activity)
        {
            activity.StudioWorkflow.RemoveActivity(activity);
            //ActivityRemoved?.Invoke(this, new ActivityEventArgs(activity));

        }

        #endregion

        /// <summary>
        /// Display new activity dialog
        /// </summary>
        /// <param name="activity"></param>
        private async Task EditActivityDialog(StudioActivity activity)
        {
            var modalConfig = new ModalOptions
            {
                Title = activity.Descriptor.DisplayName
            };

            // Launch the Properties dialog using reflection to dynamically load the activity properties component.

            // 1. Select the method to be executed
            var method = typeof(ModalService).GetMethod(nameof(ModalService.CreateDynamicModalAsync));
            // 2. Make the metod generic because CreateDynamicModalAsync is using generics
            var generic = method.MakeGenericMethod(activity.GetPropertiesComponent(), activity.GetType());
            // 3. Invoke the method and pass the required parameters
            var result = await generic.InvokeAsync(ModalService, new object[] { modalConfig, activity }) as ModalRef;

            result.OnOk = () => {

                // TODO! It may be inneficient to update the state of the entire Designer control.
                // A better alternative would be to update the state of the activity being updated.
                StateHasChanged();

                return Task.CompletedTask;
            };
        }
    }
}
