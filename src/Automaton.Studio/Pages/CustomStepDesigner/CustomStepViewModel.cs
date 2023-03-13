using Automaton.Core.Models;
using Automaton.Studio.Domain;
using Automaton.Studio.Services;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages.CustomStepDesigner;

public class CustomStepViewModel
{
    private readonly CustomStepsService customStepsService;

    public CustomStep CustomStep { get; set; } = new();

    public CustomStepDefinition CustomStepDefinition => CustomStep.Definition;

    public CustomStepViewModel
    (
        CustomStepsService customStepsService
    )
    {
        this.customStepsService = customStepsService;
    }

    public async Task Load(Guid id)
    {
        CustomStep = await customStepsService.Load(id);

        if (CustomStep.Definition == null) 
        { 
            CustomStep.Definition = new CustomStepDefinition(); 
        }
        else
        {
            if (CustomStep.Definition.CodeInputVariables == null)
            {
                CustomStep.Definition.CodeInputVariables = new List<StepVariable>();
            }

            if (CustomStep.Definition.CodeOutputVariables == null)
            {
                CustomStep.Definition.CodeOutputVariables = new List<StepVariable>();
            }
        }
    }

    public async Task Save()
    {
        await customStepsService.Update(CustomStep);
    }

    public void AddInputVariable()
    {
        var variableName = $"Variable{CustomStepDefinition.CodeInputVariables?.Count}";
        CustomStepDefinition.CodeInputVariables.Add(new StepVariable { Name = variableName });
    }

    public void DeleteInputVariable(string name)
    {
        var variable = CustomStepDefinition.CodeInputVariables.SingleOrDefault(x => x.Name == name);
        CustomStepDefinition.CodeInputVariables.Remove(variable);
    }

    public void AddOutputVariable()
    {
        var variableName = $"Variable{CustomStepDefinition.CodeOutputVariables?.Count}";
        CustomStepDefinition.CodeOutputVariables.Add(new StepVariable { Name = variableName });
    }

    public void DeleteOutputVariable(string name)
    {
        var variable = CustomStepDefinition.CodeOutputVariables.SingleOrDefault(x => x.Name == name);
        CustomStepDefinition.CodeOutputVariables.Remove(variable);
    }
}
