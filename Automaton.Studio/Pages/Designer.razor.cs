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

            DesignerViewModel.DragActivity += OnDragActivity;

            if (!string.IsNullOrEmpty(WorkflowId))
            {
                await DesignerViewModel.LoadWorkflow(WorkflowId);
            }
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
            UnselectActivities();

            dropzone.ActiveItem = e.Activity;
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
        /// Occurs when mouse is down on activity
        /// </summary>
        /// <param name="activity">Activity dropped on designer</param>
        private void OnActivityMouseDown(StudioActivity activity)
        {
            UnselectActivities();

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

            var method = typeof(ModalService).GetMethod(nameof(ModalService.CreateDynamicModalAsync));
            var generic = method.MakeGenericMethod(activity.GetDialogComponent(), activity.GetType());
            var result = await generic.InvokeAsync(ModalService, new object[] { modalConfig, activity }) as ModalRef;

            result.OnOk = () => {

                // The activity is final
                activity.PendingCreation = false;

                // TODO! It may be inneficient to update the state of the entire Designer control.
                // A better alternative would be to update the state of the activity designer component being updated.
                StateHasChanged();

                return Task.CompletedTask;
            };

            result.OnCancel = () => {

                DesignerViewModel.Workflow.Activities.Remove(activity);

                // TODO! It may be inneficient to update the state of the entire Designer control.
                // A better alternative would be to update the state of the activity designer component being updated.
                StateHasChanged();

                return Task.CompletedTask;
            };
        }

        #endregion
    }
}
