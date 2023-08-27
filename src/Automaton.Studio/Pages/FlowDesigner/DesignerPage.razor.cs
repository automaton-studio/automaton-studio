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
using System.Threading.Tasks;

namespace Automaton.Studio.Pages.FlowDesigner;

partial class DesignerPage : ComponentBase, 
    INotificationHandler<ExecuteStepNotification>,
    INotificationHandler<FlowUpdateNotification>
{
    private Designer designer;
    private Sider toolsSider;
    private Type toolsPanel = typeof(FlowSettings);

    [Parameter] public string FlowId { get; set; }
    [Inject] private ModalService ModalService { get; set; } = default!;
    [Inject] private DesignerViewModel DesignerViewModel { get; set; } = default!;
    [Inject] private FlowExplorerViewModel FlowExplorerViewModel { get; set; } = default!;

    public static event EventHandler<ExecuteStepNotification> ExecuteStep;
    public static event EventHandler<FlowUpdateNotification> FlowUpdate;

    protected override async Task OnInitializedAsync()
    {
        ExecuteStep += OnExecuteStep;
        FlowUpdate += OnFlowUpdate;

        Guid.TryParse(FlowId, out var flowId);

        await LoadFlow(flowId);

        await base.OnInitializedAsync();
    }

    public async Task RunFlow()
    {
        await DesignerViewModel.RunFlow();
    }

    public Task Handle(ExecuteStepNotification notification, CancellationToken cancellationToken)
    {
        ExecuteStep?.Invoke(this, new ExecuteStepNotification(notification.StepId));

        return Task.CompletedTask;
    }

    public Task Handle(FlowUpdateNotification notification, CancellationToken cancellationToken)
    {
        FlowUpdate?.Invoke(this, new FlowUpdateNotification());

        return Task.CompletedTask;
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

    private async Task LoadFlow(Guid flowId)
    {
        if (flowId != Guid.Empty && DesignerViewModel.Flow.Id != flowId)
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

    private void OnExecuteStep(object sender, ExecuteStepNotification e)
    {
        DesignerViewModel.SetExecutingStep(e.StepId);

        InvokeAsync(StateHasChanged);
    }

    private void OnFlowUpdate(object sender, FlowUpdateNotification e)
    {
        InvokeAsync(StateHasChanged);
    }

    public void Dispose()
    {
        ExecuteStep -= OnExecuteStep;
        FlowUpdate -= OnFlowUpdate;
    }
}
