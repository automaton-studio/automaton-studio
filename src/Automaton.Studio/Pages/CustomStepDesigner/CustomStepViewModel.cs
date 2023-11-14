using Automaton.Core.Models;
using Automaton.Studio.Domain;
using Automaton.Studio.Services;
using System.Runtime.CompilerServices;

namespace Automaton.Studio.Pages.CustomStepDesigner;

public class CustomStepViewModel
{
    private readonly CustomStepsService customStepsService;
    private readonly CustomStepExecuteService customStepExecuteService;

    private CustomStepDefinition CustomStepDefinition => CustomStep.Definition;

    public CustomStep CustomStep { get; set; } = new();

    public CustomStepViewModel(CustomStepsService customStepsService, CustomStepExecuteService customStepExecuteService)
    {
        this.customStepsService = customStepsService;
        this.customStepExecuteService = customStepExecuteService;
    }

    public async Task Load(Guid id)
    {
        CustomStep = await customStepsService.Load(id);
    }

    public async Task Save()
    {
        await customStepsService.Update(CustomStep);
    }

    public void Execute()
    {
        customStepExecuteService.Execute(CustomStep);
    }

    public void AddInputVariable()
    {
        var variableName = $"Variable{CustomStepDefinition.CodeInputVariables?.Count}";

        CustomStepDefinition.CodeInputVariables.Add(new StepVariable
        {
            Id = variableName,
            Name = variableName
        });
    }

    public void DeleteInputVariable(string name)
    {
        var variable = CustomStepDefinition.CodeInputVariables.SingleOrDefault(x => x.Name == name);
        CustomStepDefinition.CodeInputVariables.Remove(variable);
    }

    public void AddOutputVariable()
    {
        var variableName = $"Variable{CustomStepDefinition.CodeOutputVariables?.Count}";

        CustomStepDefinition.CodeOutputVariables.Add(new StepVariable
        {
            Id = variableName,
            Name = variableName 
        });
    }

    public void DeleteOutputVariable(string name)
    {
        var variable = CustomStepDefinition.CodeOutputVariables.SingleOrDefault(x => x.Name == name);
        CustomStepDefinition.CodeOutputVariables.Remove(variable);
    }
}
