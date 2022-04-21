using AntDesign;
using Automaton.Studio.Components.Drawer;
using Automaton.Studio.Domain;
using Automaton.Studio.Events;
using Automaton.Studio.Extensions;
using Automaton.Studio.Pages.Designer.Components;
using Automaton.Studio.Pages.Designer.Components.FlowExplorer;
using Automaton.Studio.Pages.Designer.Components.NewDefinition;
using Automaton.Studio.Resources;
using Automaton.Studio.ViewModels;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages.Designer
{
    partial class DesignerPage : ComponentBase
    {
        private Dropzone<Domain.Step> dropzone;

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

        protected override async Task OnInitializedAsync()
        {
            Guid.TryParse(FlowId, out var flowId);

            if (DesignerViewModel.Flow.Id != flowId)
            {
                await DesignerViewModel.LoadFlow(flowId);

                FlowExplorerViewModel.LoadDefinitions(DesignerViewModel.Flow);

                // Setup event handlers after flow is loaded
                DesignerViewModel.DragStep += OnDragStep;
                DesignerViewModel.StepAdded += OnStepAdded;
                DesignerViewModel.StepRemoved += OnStepRemoved;
            }

            await base.OnInitializedAsync();
        }

        public async Task RunFlow()
        {
            await DesignerViewModel.RunFlow();
        }

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
                DesignerViewModel.UpdateStepConnections();
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

        private async Task SaveFlow()
        {
            await DesignerViewModel.SaveFlow();
        }

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
                DesignerViewModel.DeleteStep(step);

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
            var selectedSteps = DesignerViewModel.GetSelectedSteps();

            if (selectedSteps != null)
            {
                foreach (var selectedStep in selectedSteps)
                {
                    selectedStep.Unselect();
                }
            }
        }

        private async Task OpenFlowSettings()
        {
            var options = new DrawerOptions()
            {
                Title = Labels.Settings,
                Width = 350,
                OffsetX = 50
            };

            var drawerRef = await DrawerService.CreateAsync<FlowSettings, Flow, bool>(options, DesignerViewModel.Flow);

            drawerRef.OnClosed = async result =>
            {
                await InvokeAsync(StateHasChanged);
            };
        }

        private async Task OpenFlowVariables()
        {
            var options = new DrawerOptions()
            {
                Title = Labels.Variables,
                Width = 350,
                OffsetX = 50
            };

            var drawerRef = await DrawerService.CreateAsync<FlowVariables, Flow, bool>(options, DesignerViewModel.Flow);

            drawerRef.OnClosed = async result =>
            {
                await InvokeAsync(StateHasChanged);
            };
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
                DesignerViewModel.CreateDefinition(newDefinitionModel.Name);
                FlowExplorerViewModel.RefreshDefinitions();

                DesignerViewModel.StepAdded += OnStepAdded;
                DesignerViewModel.StepRemoved += OnStepRemoved;

                StateHasChanged();

                return Task.CompletedTask;
            };
        }

        private void OnTabClose(string key)
        {
        }

        private void OnTabClick(string key)
        {
            DesignerViewModel.SetActiveDefinition(key);  
        }
    }
}
