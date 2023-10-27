using AntDesign;
using Automaton.Core.Events;
using Automaton.Studio.Domain;
using Automaton.Studio.Events;
using Automaton.Studio.Extensions;
using Automaton.Studio.Pages.FlowDesigner.Components.Drawer;
using Automaton.Studio.Pages.FlowDesigner.Components.FlowExplorer;
using Automaton.Studio.Pages.FlowDesigner.Components.NewDefinition;
using Automaton.Studio.Resources;
using MediatR;
using Microsoft.AspNetCore.Components;
using System.Threading;

namespace Automaton.Studio.Pages.FlowDesigner;

partial class DesignerPage : ComponentBase
{
    private Designer designer;
    private Sider toolsSider;
    private Type toolsPanel = typeof(FlowSettings);

    [Parameter] public string FlowId { get; set; }
    [Parameter] public string FlowName { get; set; }

    [Inject] IMediator mediator { get; set; }
    [Inject] ICourier Courier { get; set; }
    [Inject] ModalService ModalService { get; set; } = default!;
    [Inject] DesignerViewModel DesignerViewModel { get; set; } = default!;
    [Inject] FlowExplorerViewModel FlowExplorerViewModel { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        Courier.Subscribe<ExecuteStepNotification>(HandleExecuteStepNotification);
        Courier.Subscribe<FlowUpdateNotification>(HandleFlowUpdateNotification);
        Courier.Subscribe<VariableUpdateNotification>(HandleSimpleNotification);

        await LoadFlow();

        await base.OnInitializedAsync();
    }
    public async Task RunFlow()
    {
        await DesignerViewModel.RunFlow();
    }

    private void OnStepCreated(object sender, StepEventArgs e)
    {
        designer.SetActiveStep(e.Step);
    }

    private async Task OnItemDrop(StudioStep step)
    {
        if (step.IsNew)
        {
            await AddStepDialog(step);
        }
        else
        {
            DesignerViewModel.UpdateStepConnections();
        }
    }

    private async Task OnItemDoubleClick(StudioStep step)
    {
        if (!step.HasProperties)
        {
            return;
        }

        var result = await step.DisplayPropertiesDialog(ModalService);

        result.OnOk = () =>
        {
            StateHasChanged();

            return Task.CompletedTask;
        };
    }

    private void OnStepDeleted(object sender, StepEventArgs e)
    {
        StateHasChanged();
    }

    private async Task LoadFlow()
    {
        Guid.TryParse(FlowId, out var flowId);

        if (DesignerViewModel.IsFlowNotLoaded(flowId))
        {
            await DesignerViewModel.LoadFlow(flowId);

            FlowExplorerViewModel.LoadDefinitions(DesignerViewModel.Flow);

            // Setup event handlers after flow is loaded
            DesignerViewModel.StepCreated += OnStepCreated;
        }
    }

    private async Task SaveFlow()
    {
        await DesignerViewModel.SaveFlow();
    }

    private async Task AddStepDialog(StudioStep step)
    {
        if (!step.HasProperties)
        {
            designer.CompleteStep(step);

            await InvokeAsync(StateHasChanged);

            return;
        }

        var result = await step.DisplayPropertiesDialog(ModalService);

        result.OnOk = async () =>
        {
            designer.CompleteStep(step);

            await InvokeAsync(StateHasChanged);
        };

        result.OnCancel = async () =>
        {
            DesignerViewModel.DeleteStep(step);

            await InvokeAsync(StateHasChanged);
        };
    }

    private void OpenFlowSettings()
    {
        toolsSider.Collapsed = (!toolsSider.Collapsed && toolsPanel != typeof(FlowSettings)) ? 
            false : !toolsSider.Collapsed;
        toolsPanel = typeof(FlowSettings);
    }

    private void OpenFlowVariables()
    {
        toolsSider.Collapsed = (!toolsSider.Collapsed && toolsPanel != typeof(FlowVariables)) ? 
            false : !toolsSider.Collapsed;
        toolsPanel = typeof(FlowVariables);
    }

    private void OpenFlowLogs()
    {
        toolsSider.Collapsed = (!toolsSider.Collapsed && toolsPanel != typeof(FlowLogs)) ?
            false : !toolsSider.Collapsed;
        toolsPanel = typeof(FlowLogs);
    }

    private async Task OnDefinitionAddClick()
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
            var definition = DesignerViewModel.CreateDefinition(newDefinitionModel.Name);
            DesignerViewModel.SetActiveDefinition(definition);
            FlowExplorerViewModel.RefreshDefinitions();

            StateHasChanged();

            return Task.CompletedTask;
        };
    }

    private void OnTabClick(string key)
    {
        DesignerViewModel.SetActiveDefinition(key);
    }

    private void OnTabClose(string key)
    {
    }

    private void HandleExecuteStepNotification(ExecuteStepNotification notification, CancellationToken cancellationToken)
    {
        if(cancellationToken.IsCancellationRequested) 
            return;

        DesignerViewModel.SetExecutingStep(notification.StepId);

        InvokeAsync(StateHasChanged);
    }

    private void HandleFlowUpdateNotification(FlowUpdateNotification notification, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
            return;

        InvokeAsync(StateHasChanged);
    }

    private void HandleSimpleNotification(VariableUpdateNotification notification, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
            return;

        DesignerViewModel.Flow.Variables[notification.Variable.Name] = notification.Variable;

        InvokeAsync(StateHasChanged);
    }

    public void Dispose()
    {
        Courier.UnSubscribe<ExecuteStepNotification>(HandleExecuteStepNotification);
        Courier.UnSubscribe<FlowUpdateNotification>(HandleFlowUpdateNotification);
        Courier.UnSubscribe<VariableUpdateNotification>(HandleSimpleNotification);
    }
}
