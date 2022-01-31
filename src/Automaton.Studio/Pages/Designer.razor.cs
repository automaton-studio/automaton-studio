using AntDesign;
using Automaton.Studio.Components;
using Automaton.Studio.Components.Explorer.FlowExplorer;
using Automaton.Studio.Components.NewDefinition;
using Automaton.Studio.Domain;
using Automaton.Studio.Events;
using Automaton.Studio.Extensions;
using Automaton.Studio.Resources;
using Automaton.Studio.ViewModels;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages
{
    partial class Designer : ComponentBase
    {
        #region Members

        private Dropzone<Domain.Step> dropzone;

        #endregion

        #region DI

        [Inject]
        private ModalService ModalService { get; set; } = default!;

        [Inject]
        private IDesignerViewModel DesignerViewModel { get; set; } = default!;

        [Inject]
        private IFlowExplorerViewModel FlowExplorerViewModel { get; set; } = default!;

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
            if (!string.IsNullOrEmpty(FlowId) && 
                DesignerViewModel.Flow.Id != FlowId)
            {
                await DesignerViewModel.LoadFlow(FlowId);

                // Setup event handlers after workflow is loaded
                DesignerViewModel.DragStep += OnDragStep;
                DesignerViewModel.StepAdded += OnStepAdded;
                DesignerViewModel.StepRemoved += OnStepRemoved;
            }

            await base.OnInitializedAsync();
        }

        #endregion

        #region Private Methods

        #region Event Handlers

        private void OnDragStep(object sender, StepEventArgs e)
        {
            dropzone.ActiveItem = e.Step;

            // Unselect all the previous selected activities
            UnselectSteps();

            // Select the step being dragged
            dropzone.ActiveItem.Select();
        }

        private async Task OnStepDrop(Domain.Step step)
        {
            if (!step.IsFinal())
            {
                await NewStepDialog(step);
            }
            else
            {
                DesignerViewModel.UpdateStepConnections(step);
            }
        }

        private void OnStepMouseDown(Domain.Step step)
        {
            // Unselect all the previous selected activities
            UnselectSteps();

            // Select the one under the mouse cursor
            step.Select();
        }

        private async Task OnStepDoubleClick(Domain.Step step)
        {
            var result = await step.DisplayPropertiesDialog(ModalService);

            result.OnOk = () =>
            {
                StateHasChanged();

                return Task.CompletedTask;
            };
        }

        private void OnDropzoneMouseDown()
        {
            UnselectSteps();
        }

        private void OnStepAdded(object sender, StepEventArgs e)
        {
            StateHasChanged();
        }

        private void OnStepRemoved(object sender, StepEventArgs e)
        {
            StateHasChanged();
        }

        #endregion

        #region Workflow Actions

        public async Task RunWorkflow()
        {
            //await DesignerViewModel.RunWorkflow();
        }

        private async Task SaveFlow()
        {
            await DesignerViewModel.SaveFlow();
        }

        #endregion

        private async Task NewStepDialog(Domain.Step step)
        {
            var result = await step.DisplayPropertiesDialog(ModalService);

            result.OnOk = () =>
            {
                DesignerViewModel.FinalizeStep(step);

                // TODO! It may be inneficient to update the state of the entire Designer control.
                // A better alternative would be to update the state of the step being updated.
                StateHasChanged();

                return Task.CompletedTask;
            };

            result.OnCancel = () =>
            {
                //DesignerViewModel.DeleteActivity(activity);

                // TODO! It may be inneficient to update the state of the entire Designer control.
                // A better alternative would be to update the state of the activity being updated.
                StateHasChanged();

                return Task.CompletedTask;
            };
        }

        /// <summary>
        /// Unselect all selected activities
        /// </summary>
        private void UnselectSteps()
        {
            var selectedSteps = DesignerViewModel.GetSelectedSteps(); ;

            if (selectedSteps != null)
            {
                foreach (var selectedStep in selectedSteps)
                {
                    selectedStep.Unselect();
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
            var newDefinitionModel = new NewDefinitionModel
            {
                ExistingNames = DesignerViewModel.GetDefinitionNames()
            };

            var newDefinitionDialog = await ModalService.CreateModalAsync<NewDefinitionDialog, NewDefinitionModel>
            (
                new ModalOptions { Title = Labels.DefinitionName }, newDefinitionModel
            );

            newDefinitionDialog.OnOk = () =>
            {
                DesignerViewModel.NewDefinition(newDefinitionModel.Name);
                FlowExplorerViewModel.RefreshDefinitions();

                DesignerViewModel.StepAdded += OnStepAdded;
                DesignerViewModel.StepRemoved += OnStepRemoved;

                StateHasChanged();

                return Task.CompletedTask;
            };
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
