
using AntDesign;
using Automaton.Core.Models;
using Automaton.Studio.Domain;
using Automaton.Studio.Pages.FlowDesigner.Components.NewVariable;
using Automaton.Studio.Resources;
using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages.FlowDesigner.Components.Drawer;

public partial class FlowVariables : ComponentBase
{
    [CascadingParameter]
    private StudioFlow Flow { get; set; }

    private FluentValidationValidator fluentValidationValidator;

    [Inject] private ModalService ModalService { get; set; } = default!;

    private IEnumerable<StepVariable> Variables
    {
        get
        {
            return Flow.Variables.Select(x => new StepVariable
            {
                Name = x.Key,
            }).OrderBy(x => x.Name);
        }
    }

    private IEnumerable<StepVariable> InputVariables
    {
        get
        {
            return Flow.InputVariables.Select(x => new StepVariable
            {
                Name = x.Key,
                Value = x.Value.ToString()
            }).OrderBy(x => x.Name);
        }
    }

    private IEnumerable<StepVariable> OutputVariables
    {
        get
        {
            return Flow.OutputVariables.Select(x => new StepVariable
            {
                Name = x.Key,
                Value = x.Value.ToString()
            }).OrderBy(x => x.Name);
        }
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
    }

    public async Task AddOutputVariable()
    {
        var newDefinitionModel = new VariableModel
        {
            ExistingNames = Flow.GetOutputVariableNames()
        };

        var newVariableDialog = await ModalService.CreateModalAsync<VariableDialog, VariableModel>
        (
            new ModalOptions { Title = Labels.Variable }, newDefinitionModel
        );

        newVariableDialog.OnOk = () =>
        {
            Flow.SetOutputVariable(newDefinitionModel.Name, newDefinitionModel.Value);

            //StateHasChanged();

            return Task.CompletedTask;
        };
    }

    public async Task EditOutputVariable(StepVariable variable)
    {
        var outputVariableNames = Flow.GetOutputVariableNames();
        var existingOutputVariables = outputVariableNames.Where(x => !x.Equals(variable.Name, StringComparison.OrdinalIgnoreCase));

        var updatedVariable = new VariableModel
        {
            ExistingNames = existingOutputVariables,
            Name = variable.Name,
            Value = variable.Value
        };

        var newVariableDialog = await ModalService.CreateModalAsync<VariableDialog, VariableModel>
        (
            new ModalOptions { Title = Labels.Variable }, updatedVariable
        );

        newVariableDialog.OnOk = () =>
        {
            if (!variable.Name.Equals(updatedVariable.Name, StringComparison.OrdinalIgnoreCase))
            {
                Flow.DeleteOutputVariable(variable.Name);
            }

            Flow.SetOutputVariable(updatedVariable.Name, updatedVariable.Value);

            //StateHasChanged();

            return Task.CompletedTask;
        };
    }

    public void DeleteOutputVariable(StepVariable variable)
    {
        Flow.OutputVariables.Remove(variable.Name);
    }

    public async Task AddInputVariable()
    {
        var newDefinitionModel = new VariableModel
        {
            ExistingNames = Flow.GetInputVariableNames()
        };

        var newVariableDialog = await ModalService.CreateModalAsync<VariableDialog, VariableModel>
        (
            new ModalOptions { Title = Labels.Variable }, newDefinitionModel
        );

        newVariableDialog.OnOk = () =>
        {
            Flow.SetInputVariable(newDefinitionModel.Name, newDefinitionModel.Value);

            //StateHasChanged();

            return Task.CompletedTask;
        };
    }

    public async Task EditInputVariable(StepVariable variable)
    {
        var inputVariableNames = Flow.GetInputVariableNames();
        var existingInputVariables = inputVariableNames.Where(x => !x.Equals(variable.Name, StringComparison.OrdinalIgnoreCase));

        var updatedVariable = new VariableModel
        {
            ExistingNames = existingInputVariables,
            Name = variable.Name,
            Value = variable.Value
        };

        var newVariableDialog = await ModalService.CreateModalAsync<VariableDialog, VariableModel>
        (
            new ModalOptions { Title = Labels.Variable }, updatedVariable
        );

        newVariableDialog.OnOk = () =>
        {
            if (!variable.Name.Equals(updatedVariable.Name, StringComparison.OrdinalIgnoreCase))
            {
                Flow.DeleteInputVariable(variable.Name);
            }

            Flow.SetInputVariable(updatedVariable.Name, updatedVariable.Value);

            //StateHasChanged();

            return Task.CompletedTask;
        };
    }

    public void DeleteInputVariable(StepVariable variable)
    {
        Flow.InputVariables.Remove(variable.Name);
    }

    public async Task Cancel()
    {
        //await CloseFeedbackAsync();
    }
}
