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

        #region Event Handlers

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
        private async Task OnDropzoneMouseDown()
        {
            // Unselect all the previous selected activities
            UnselectActivities();
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
                await NewActivityDialog(activity);
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

        #endregion


        #region Workflow Actions

        /// <summary>
        /// Run workflow
        /// </summary>
        /// <returns></returns>
        public async Task RunWorkflow()
        {
            await DesignerViewModel.RunWorkflow();
        }

        /// <summary>
        /// Save workflow
        /// </summary>
        private async Task SaveWorkflow()
        {
            await DesignerViewModel.SaveWorkflow();
        }

        #endregion

        /// <summary>
        /// Display new activity dialog
        /// </summary>
        /// <param name="activity"></param>
        private async Task NewActivityDialog(StudioActivity activity)
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

                // The activity was created and it's final
                activity.PendingCreation = false;

                // TODO! It may be inneficient to update the state of the entire Designer control.
                // A better alternative would be to update the state of the activity being updated.
                StateHasChanged();

                return Task.CompletedTask;
            };

            result.OnCancel = () => {

                // Activity is removed from workflow
                DesignerViewModel.StudioWorkflow.Activities.Remove(activity);

                // TODO! It may be inneficient to update the state of the entire Designer control.
                // A better alternative would be to update the state of the activity being updated.
                StateHasChanged();

                return Task.CompletedTask;
            };
        }

        /// <summary>
        /// Unselect all selected activities
        /// </summary>
        private void UnselectActivities()
        {
            var selectedActivities = DesignerViewModel.StudioWorkflow.Activities.Where(x => x.IsSelected());

            if (selectedActivities != null)
            {
                foreach (var selectedActivity in selectedActivities)
                {
                    selectedActivity.Unselect();
                }
            }
        }

        #endregion
    }
}
