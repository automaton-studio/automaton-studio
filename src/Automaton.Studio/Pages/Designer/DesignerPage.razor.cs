using AntDesign;
using Automaton.Studio.Domain;
using Automaton.Studio.Events;
using Automaton.Studio.Extensions;
using Automaton.Studio.Pages.Designer.Components;
using Automaton.Studio.Pages.Designer.Components.Drawer;
using Automaton.Studio.Pages.Designer.Components.FlowExplorer;
using Automaton.Studio.Pages.Designer.Components.NewDefinition;
using Automaton.Studio.Resources;
using Automaton.Studio.Services;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages.Designer;

partial class DesignerPage : ComponentBase
{
    private Dropzone dropzone;

    [Inject] private ModalService ModalService { get; set; } = default!;

    [Inject] private DesignerViewModel DesignerViewModel { get; set; } = default!;

    [Inject] private FlowExplorerViewModel FlowExplorerViewModel { get; set; } = default!;

    [Inject] private DrawerService DrawerService { get; set; } = default!;

    [Inject] public NavMenuService NavMenuService { get; set; }

    [Parameter] public string FlowId { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Guid.TryParse(FlowId, out var flowId);

        await LoadFlow(flowId);

        await base.OnInitializedAsync();
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
        if (!step.IsFinal())
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

    private void OnStepAdded(object sender, StepEventArgs e)
    {
        StateHasChanged();
    }

    private void OnStepRemoved(object sender, StepEventArgs e)
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
            DesignerViewModel.StepAdded += OnStepAdded;
            DesignerViewModel.StepRemoved += OnStepRemoved;
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
            DesignerViewModel.FinalizeStep(step);
            return;
        }

        var result = await step.DisplayPropertiesDialog(ModalService);

        result.OnOk = () =>
        {
            DesignerViewModel.FinalizeStep(step);

            StateHasChanged();

            return Task.CompletedTask;
        };

        result.OnCancel = () =>
        {
            DesignerViewModel.DeleteStep(step);

            StateHasChanged();

            return Task.CompletedTask;
        };
    }

    private async Task OpenFlowSettings()
    {
        var options = new DrawerOptions()
        {
            Title = Labels.Settings,
            Width = 350
        };

        var drawerRef = await DrawerService.CreateAsync<FlowSettings, StudioFlow, bool>(options, DesignerViewModel.Flow);

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
            Width = 350
        };

        var drawerRef = await DrawerService.CreateAsync<FlowVariables, StudioFlow, bool>(options, DesignerViewModel.Flow);

        drawerRef.OnClosed = async result =>
        {
            await InvokeAsync(StateHasChanged);
        };
    }

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
