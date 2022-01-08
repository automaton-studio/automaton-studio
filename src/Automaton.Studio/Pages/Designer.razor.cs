using AntDesign;
using Automaton.Studio.Components;
using Automaton.Studio.Conductor;
using Automaton.Studio.Events;
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

        private Guid currentFlowId;
        private Dropzone<Conductor.Step> dropzone;
        private Definition solutionFlow;

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
        public string FlowId { get; set; }

        #endregion

        #region Overrides

        protected override async Task OnInitializedAsync()
        {
            if (!string.IsNullOrEmpty(FlowId))
            {
                await DesignerViewModel.LoadFlow(FlowId);
            }

            // Setup event handlers after workflow is loaded
            DesignerViewModel.DragActivity += OnDragActivity;
            if (DesignerViewModel.StudioFlow != null)
            {
                DesignerViewModel.ActivityAdded += OnActivityAdded;
                DesignerViewModel.ActivityRemoved += OnActivityRemoved;
            }

            await base.OnInitializedAsync();
        }

        #endregion

        #region Private Methods

        #region Event Handlers

        /// <summary>
        /// Occurs when an activity is dragged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDragActivity(object sender, StepEventArgs e)
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
        private async Task OnActivityDrop(Conductor.Step activity)
        {
            // When activity was already created don't display create dialog when OnDrop event occurs
            if (activity.PendingCreation)
            {
                await NewActivityDialog(activity);
            }
            else
            {
                activity.UpdateConnections();
            }
        }

        /// <summary>
        /// Occurs when mouse is down on activity.
        /// </summary>
        /// <param name="activity">Activity dropped on designer</param>
        private void OnActivityMouseDown(Conductor.Step activity)
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
        private async Task OnActivityDoubleClick(Conductor.Step activity)
        {
            //var result = await activity.EditActivityDialog(ModalService);

            //result.OnOk = () =>
            //{

            //    StateHasChanged();

            //    return Task.CompletedTask;
            //};
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
        private void OnActivityAdded(object sender, StepEventArgs e)
        {
            StateHasChanged();
        }

        /// <summary>
        /// Occurs when an activity is removed from the workflow
        /// </summary>
        private void OnActivityRemoved(object sender, StepEventArgs e)
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
            //await DesignerViewModel.RunWorkflow();
        }

        /// <summary>
        /// Save workflow
        /// </summary>
        private async Task SaveFlow()
        {
            await DesignerViewModel.SaveFlow();
        }

        #endregion

        /// <summary>
        /// Display new activity dialog
        /// </summary>
        /// <param name="activity"></param>
        private async Task NewActivityDialog(Conductor.Step activity)
        {
            var modalConfig = new ModalOptions
            {
                Title = activity.Id
            };

            //// Launch the Properties dialog using reflection to dynamically load the activity properties component.

            //// 1. Select the method to be executed
            //var method = typeof(ModalService).GetMethod(nameof(ModalService.CreateDynamicModalAsync));
            //// 2. Make the metod generic because CreateDynamicModalAsync is using generics
            //var generic = method.MakeGenericMethod(activity.GetPropertiesComponent(), activity.GetType());
            //// 3. Invoke the method and pass the required parameters
            //var result = await generic.InvokeAsync(ModalService, new object[] { modalConfig, activity }) as ModalRef;

            //result.OnOk = () =>
            //{

            //    DesignerViewModel.FinalizeActivity(activity);

            //    // TODO! It may be inneficient to update the state of the entire Designer control.
            //    // A better alternative would be to update the state of the activity being updated.
            //    StateHasChanged();

            //    return Task.CompletedTask;
            //};

            //result.OnCancel = () =>
            //{

            //    DesignerViewModel.DeleteActivity(activity);

            //    // TODO! It may be inneficient to update the state of the entire Designer control.
            //    // A better alternative would be to update the state of the activity being updated.
            //    StateHasChanged();

            //    return Task.CompletedTask;
            //};
        }

        /// <summary>
        /// Unselect all selected activities
        /// </summary>
        private void UnselectActivities()
        {
            var selectedActivities = DesignerViewModel.StudioFlow.Steps.Where(x => x.IsSelected());

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
            //var options = new DrawerOptions()
            //{
            //    Title = "Workflow Settings",
            //    Width = 350,
            //    OffsetX = 50
            //};

            //var drawerRef = await DrawerService.CreateAsync<WorkflowDetails, StudioWorkflow, bool>(options, DesignerViewModel.StudioFlow.ActiveWorkflow);

            //drawerRef.OnClosed = async result =>
            //{
            //    await InvokeAsync(StateHasChanged);
            //};
        }

        /// <summary>
        /// Open workflow settings drawer
        /// </summary>
        private async Task OpenWorkflowVariables()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a new workflow tab
        /// </summary>
        private async Task OnWorkflowAddClick()
        {
            //var workflowNameModel = new WorkflowNameModel
            //{
            //    ExistingNames = SolutionFlowViewModel.WorkflowNames
            //};

            //var modalRef = await ModalService.CreateModalAsync<WorkflowNameDialog, WorkflowNameModel>
            //(
            //    new ModalOptions { Title = Labels.RenameWorkflow }, workflowNameModel
            //);

            //modalRef.OnOk = async () =>
            //{
            //    // Add a new workflow to DesignerViewModel
            //    var newStudioWorkflow = new StudioWorkflow { Name = workflowNameModel.Name };
            //    DesignerViewModel.AddWorkflow(newStudioWorkflow);
            //    DesignerViewModel.ActivityAdded += OnActivityAdded;
            //    DesignerViewModel.ActivityRemoved += OnActivityRemoved;

            //    // Add a new workflow to SolutionFlowViewModel
            //    var newWorkflowModel = new WorkflowModel { Name = workflowNameModel.Name, IsStartup = true };
            //    SolutionFlowViewModel.AddWorkflow(newWorkflowModel);

            //    // Refresh Flow page otherwise tabs are not updated
            //    StateHasChanged();

            //    if (solutionFlow != null)
            //    {
            //        // Refresh SolutionFlow component otherwise the new workflow isn't displayed
            //        solutionFlow.UpdateState();
            //    }
            //};
        }

        /// <summary>
        /// Closes a workflow tab
        /// </summary>
        private void OnWorkflowTabClose(string key)
        {
        }

        #endregion
    }
}
