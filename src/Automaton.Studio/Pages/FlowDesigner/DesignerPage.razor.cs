using AntDesign;
using Automaton.Core.Events;
using Automaton.Studio.Domain;
using Automaton.Studio.Events;
using Automaton.Studio.Extensions;
using Automaton.Studio.Pages.FlowDesigner.Components;
using Automaton.Studio.Pages.FlowDesigner.Components.Drawer;
using Automaton.Studio.Pages.FlowDesigner.Components.FlowExplorer;
using Automaton.Studio.Pages.FlowDesigner.Components.NewDefinition;
using Automaton.Studio.Resources;
using MediatR;
using Microsoft.AspNetCore.Components;
using System.Threading;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages.FlowDesigner;

partial class DesignerPage : INotificationHandler<ExecuteStepNotification>
{
    private Dropzone dropzone;
    private Sider toolsSider;
    private Type toolsPanel = typeof(FlowSettings);
    public static event EventHandler<ExecuteStepEventArgs> OnExecuteStep;

    [Parameter] public string FlowId { get; set; }

    [Inject] private ModalService ModalService { get; set; } = default!;
    [Inject] private DesignerViewModel DesignerViewModel { get; set; } = default!;
    [Inject] private FlowExplorerViewModel FlowExplorerViewModel { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        Guid.TryParse(FlowId, out var flowId);

        await LoadFlow(flowId);

        await base.OnInitializedAsync();

        OnExecuteStep += (sender, changed) =>
        {
            DesignerViewModel.SetExecutingStep(changed.StepId);
            InvokeAsync(StateHasChanged);
        };
    }

    public async Task RunFlow()
    {
        await DesignerViewModel.RunFlow();
    }

    private void OnStepCreated(object sender, StepEventArgs e)
    {
        dropzone.SetActiveStep(e.Step);
    }

    private async Task OnItemDrop(StudioStep step)
    {
        if (step.IsNew)
        {
            await NewStepDialog(step);
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

    private async Task NewStepDialog(StudioStep step)
    {
        if (!step.HasProperties)
        {
            step.InvokeFinalize();
            dropzone.SelectStep(step);

            return;
        }

        var result = await step.DisplayPropertiesDialog(ModalService);

        result.OnOk = () =>
        {
            step.InvokeFinalize();
            dropzone.SelectStep(step);

            return Task.CompletedTask;
        };

        result.OnCancel = () =>
        {
            DesignerViewModel.DeleteStep(step);

            return Task.CompletedTask;
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

    public Task Handle(ExecuteStepNotification notification, CancellationToken cancellationToken)
    {
        OnExecuteStep?.Invoke(this, new ExecuteStepEventArgs() { StepId = notification.StepId });
        return Task.CompletedTask;
    }
}
