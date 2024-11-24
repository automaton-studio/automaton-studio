
using AntDesign;
using Automaton.Core.Enums;
using Automaton.Core.Models;
using Automaton.Studio.Domain;
using Automaton.Studio.Pages.FlowDesigner.Components.NewVariable;
using Automaton.Studio.Pages.FlowDesigner.Components.ViewVariable;
using Automaton.Studio.Resources;
using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;

namespace Automaton.Studio.Pages.FlowDesigner.Components.Drawer;

public partial class FlowVariables : ComponentBase
{
    private FluentValidationValidator fluentValidationValidator;

    [CascadingParameter] StudioFlow Flow { get; set; }
    [Inject] ModalService ModalService { get; set; } = default!;

    public IEnumerable<VariableType> VariableTypes { get; } = Enum.GetValues<VariableType>();

    private IEnumerable<StepVariable> Variables
    {
        get
        {
            return Flow.Variables.Select(x => new StepVariable
            {
                Id = x.Key,
                Name = x.Value.Name,
                Value = x.Value.Value,
                Description = x.Value.Description
            }).OrderBy(x => x.Name);
        }
    }

    private IEnumerable<StepVariable> InputVariables
    {
        get
        {
            return Flow.InputVariables.Select(x => new StepVariable
            {
                Id = x.Key,
                Name = x.Value.Name,
                Value = x.Value.Value,
                Description = x.Value.Description
            }).OrderBy(x => x.Name);
        }
    }

    private IEnumerable<StepVariable> OutputVariables
    {
        get
        {
            return Flow.OutputVariables.Select(x => new StepVariable
            {
                Id = x.Key,
                Name = x.Value.Name,
                Value = x.Value.Value,
                Description = x.Value.Description
            }).OrderBy(x => x.Name);
        }
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
    }

    public async Task AddOutputVariable()
    {
        var newVariable = new VariableModel
        {
            ExistingNames = Flow.GetOutputVariableNames()
        };

        var newVariableDialog = await ModalService.CreateModalAsync<VariableDialog, VariableModel>
        (
            new ModalOptions { Title = Labels.Variable }, newVariable
        );

        newVariableDialog.OnOk = () =>
        {
            Flow.SetOutputVariable(new StepVariable 
            { 
                Id = Guid.NewGuid().ToString(),
                Name = newVariable.Name, 
                Value = newVariable.Value,
                Type = newVariable.Type,
                Description = newVariable.Description
            });

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
            Type = variable.Type,
            Value = variable.Value,
            Description = variable.Description
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

            Flow.SetOutputVariable(new StepVariable { Id = Guid.NewGuid().ToString(), Name = updatedVariable.Name, Value = updatedVariable.Value });

            return Task.CompletedTask;
        };
    }

    public void DeleteOutputVariable(StepVariable variable)
    {
        Flow.OutputVariables.Remove(variable.Name);
    }

    public async Task AddInputVariable()
    {
        var newVariable = new VariableModel
        {
            ExistingNames = Flow.GetInputVariableNames()
        };

        var newVariableDialog = await ModalService.CreateModalAsync<VariableDialog, VariableModel>
        (
            new ModalOptions { Title = Labels.Variable }, newVariable
        );

        newVariableDialog.OnOk = () =>
        {
            Flow.SetInputVariable(new StepVariable 
            {
                Id = Guid.NewGuid().ToString(),
                Name = newVariable.Name, 
                Type = newVariable.Type, 
                Value = newVariable.Value,
                Description = newVariable.Description
            });

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
            Value = variable.Value,
            Type = variable.Type,
            Description = variable.Description
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

            Flow.SetInputVariable(new StepVariable 
            {
                Name = updatedVariable.Name,
                Type = updatedVariable.Type,
                Value = updatedVariable.Value,
                Description = updatedVariable.Description
            });

            return Task.CompletedTask;
        };
    }

    public void DeleteInputVariable(StepVariable variable)
    {
        Flow.InputVariables.Remove(variable.Name);
    }

    public async Task ViewVariable(StepVariable variable)
    {
        var viewVariableModel = new ViewVariableModel
        {
            Variable = variable,
            Flow = Flow
        };

        var newVariableDialog = await ModalService.CreateModalAsync<ViewVariableDialog, ViewVariableModel>
        (
            new ModalOptions { Title = Labels.VariableValue }, viewVariableModel
        );

        newVariableDialog.OnOk = () =>
        {
            return Task.CompletedTask;
        };
    }
}
