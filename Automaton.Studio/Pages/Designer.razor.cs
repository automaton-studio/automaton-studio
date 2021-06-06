using AntDesign;
using Automaton.Studio.Activity;
using Automaton.Studio.Events;
using Automaton.Studio.Extensions;
using Automaton.Studio.ViewModels;
using Microsoft.AspNetCore.Components;
using Plk.Blazor.DragDrop;
using System.Linq;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages
{
    partial class Designer : ComponentBase
    {
        #region Members

        private Dropzone<StudioActivity>? dropzone;

        #endregion

        #region DI

        [Inject] ModalService ModalService { get; set; } = default!;
        [Inject] private IDesignerViewModel DesignerViewModel { get; set; } = default!;

        #endregion

        #region Params

        [Parameter] public string WorkflowId { get; set; }

        #endregion

        #region Overrides

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            await DesignerViewModel.Initialize(WorkflowId);
            DesignerViewModel.DragActivity += OnDragActivity;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Occurs when an activity is dragged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDragActivity(object? sender, DragActivityEventArgs e)
        {
            dropzone.ActiveItem = e.Activity;

            // Unselect all the previous selected activities
            UnselectActivities();

            // Select the activity being dragged
            dropzone.ActiveItem.Select();
        }

        /// <summary>
        /// Occurs when a new activity is dropped on designer
        /// </summary>
        /// <param name="activity">Activity dropped on designer</param>
        private async Task OnActivityDrop(StudioActivity activity)
        {
            // When activity was already created don't display create dialog when OnDrop event occurs
            if (activity.PendingCreation)
            {
                await ShowActivityDialog(activity);
            }
        }

        /// <summary>
        /// Occurs when mouse is down on activity.
        /// </summary>
        /// <param name="activity">Activity dropped on designer</param>
        private void OnActivityMouseDown(StudioActivity activity)
        {
            // Unselect all the previous selected activities
            UnselectActivities();

            // Select the one under the mouse cursor
            activity.Select();
        }

        /// <summary>
        /// Unselect all selected activities
        /// </summary>
        private void UnselectActivities()
        {
            var selectedActivities = DesignerViewModel.Workflow.Activities.Where(x => x.IsSelected());

            if (selectedActivities != null)
            {
                foreach (var selectedActivity in selectedActivities)
                {
                    selectedActivity.Unselect();
                }
            }
        }

        /// <summary>
        /// Display activity dialog
        /// </summary>
        /// <param name="activity"></param>
        private async Task ShowActivityDialog(StudioActivity activity)
        {
            var modalConfig = new ModalOptions
            {
                Title = activity.Descriptor.DisplayName
            };

            // We need to launch the Properties dialog using reflexion to load the right activity properties content
            // 1. Select the method to be executed, in this case AndDesign's ModalService.CreateDynamicModalAsync
            // CreateDynamicModalAsync is an exact copy of ModalService.CreateModalAsync that's used here to avoid
            // relexion engine being confused about the write method to execute (there are a few CreateModalAsync)
            var method = typeof(ModalService).GetMethod(nameof(ModalService.CreateDynamicModalAsync));
            // 2. Make the metod generic (CreateDynamicModalAsync is using generics)
            var generic = method.MakeGenericMethod(activity.GetPropertiesComponent(), activity.GetType());
            // 3. Invoke the method and pass the required parameters
            var result = await generic.InvokeAsync(ModalService, new object[] { modalConfig, activity }) as ModalRef;

            result.OnOk = () => {

                // The activity was created and it's final
                activity.PendingCreation = false;

                return Task.CompletedTask;
            };

            result.OnCancel = () => {

                // Activity is removed from workflow
                DesignerViewModel.Workflow.Activities.Remove(activity);

                return Task.CompletedTask;
            };

            // TODO! It may be inneficient to update the state of the entire Designer control.
            // A better alternative would be to update the state of the activity being updated.
            StateHasChanged();
        }

        #endregion
    }
}
