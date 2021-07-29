using AntDesign;
using Automaton.Studio.Activity;
using Automaton.Studio.Activity.Extensions;
using Automaton.Studio.Components;
using Automaton.Studio.DragDrop;
using Automaton.Studio.ViewModels;
using Microsoft.AspNetCore.Components;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages
{
    partial class Designer : ComponentBase
    {
        #region Members

        private Dropzone<StudioActivity> dropzone;

        #endregion

        #region DI

        [Inject] 
        private ModalService ModalService { get; set; } = default!;

        [Inject] 
        private IDesignerViewModel DesignerViewModel { get; set; } = default!;

        [Inject]
        private DrawerService DrawerService { get; set; } = default!;

        #endregion

        #region Params

        [Parameter] 
        public string WorkflowId { get; set; }

        #endregion

        #region Overrides

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            if (!string.IsNullOrEmpty(WorkflowId))
            {
                await DesignerViewModel.LoadWorkflow(WorkflowId);
            }

            // Setup event handlers after workflow is loaded
            DesignerViewModel.DragActivity += OnDragActivity;
            DesignerViewModel.StudioWorkflow.ActivityAdded += OnActivityAdded;
            DesignerViewModel.StudioWorkflow.ActivityRemoved += OnActivityRemoved;

        }

        #endregion

        #region Private Methods

        #region Event Handlers

        /// <summary>
        /// Occurs when an activity is dragged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDragActivity(object sender, ActivityEventArgs e)
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

        /// <summary>
        /// Occurs when double click over an activity.
        /// </summary>
        /// <param name="activity">Clicked activity</param>
        private async Task OnActivityDoubleClick(StudioActivity activity)
        {
            var result = await activity.EditActivityDialog(ModalService);

            result.OnOk = () => {

                StateHasChanged();

                return Task.CompletedTask;
            };
        }

        /// <summary>
        /// Occurs when a new activity is dropped on designer
        /// </summary>
        /// <param name="activity">Activity dropped on designer</param>
        private void OnDropzoneMouseDown()
        {
            // Unselect all the previous selected activities
            UnselectActivities();
        }

        /// <summary>
        /// Occurs when an activity is added to the workflow
        /// </summary>
        private void OnActivityAdded(object sender, ActivityEventArgs e)
        {
            StateHasChanged();
        }

        /// <summary>
        /// Occurs when an activity is removed from the workflow
        /// </summary>
        private void OnActivityRemoved(object sender, ActivityEventArgs e)
        {
            StateHasChanged();
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

                DesignerViewModel.FinalizeActivity(activity);

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

        /// <summary>
        /// Open workflow settings drawer
        /// </summary>
        private async Task OpenWorkflowSettings()
        {
            var options = new DrawerOptions()
            {
                Title = "Workflow Settings",
                Width = 350,
                OffsetX = 50
            };

            var drawerRef = await DrawerService.CreateAsync<WorkflowDetails, StudioWorkflow, bool>(options, DesignerViewModel.StudioWorkflow);

            drawerRef.OnClosed = async result =>
            {                   
                await InvokeAsync(StateHasChanged);
            };
        }

        /// <summary>
        /// Open workflow settings drawer
        /// </summary>
        private async Task OpenWorkflowVariables()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
