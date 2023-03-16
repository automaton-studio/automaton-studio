using Automaton.Core.Models;
using Automaton.Studio.Domain;
using Automaton.Studio.Extensions;
using Automaton.Studio.Pages.FlowDesigner.Components.StepExplorer;
using Automaton.Studio.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Threading.Tasks;
using static IronPython.Modules._ast;

namespace Automaton.Studio.Factories;

public class StepFactory
{
    private const string StepsAssembly = "Automaton.Studio";

    private readonly IServiceProvider serviceProvider;
    private readonly CustomStepsService customStepsService;
    private readonly StepTypeDescriptor stepTypeDescriptor;
    private readonly IDictionary<string, StepExplorerModel> explorerSteps = new Dictionary<string, StepExplorerModel>();
    private readonly IDictionary<string, Type> solutionTypes = new Dictionary<string, Type>();

    public StepFactory(StepTypeDescriptor stepTypeDescriptor, IServiceProvider serviceProvider, CustomStepsService customStepsService)
    {
        this.serviceProvider = serviceProvider;
        this.stepTypeDescriptor = stepTypeDescriptor;
        this.customStepsService = customStepsService;

        LoadSteps();
    }

    public IEnumerable<StepExplorerModel> GetSteps()
    {
        return explorerSteps.Values;
    }

    public StudioStep CreateStep(string name)
    {
        var descriptor = stepTypeDescriptor.Describe(solutionTypes[name]);
        var step = serviceProvider.GetService(solutionTypes[name]) as StudioStep;

        step.Setup(descriptor);

        return step;
    }

    public StudioStep CreateStudioStep(Step step)
    {
        var studioStep = serviceProvider.GetService(solutionTypes[step.Type]) as StudioStep;

        studioStep.Setup(step);

        return studioStep;
    }

    public StudioStep CreateCustomStep(CustomStepExplorerModel customStepModel)
    {
        var descriptor = new StepDescriptor
        {
            Name = customStepModel.Name,
            Type = customStepModel.Type,
            DisplayName = customStepModel.DisplayName,
            Description = customStepModel.Description,
            Category = customStepModel.Category,
            Icon = customStepModel.Icon,
            MoreInfo = customStepModel.MoreInfo,
            VisibleInExplorer = customStepModel.VisibleInExplorer
        };

        var step = new Steps.Custom.CustomStep
        {
            Code = customStepModel.Definition.Code,
            CodeInputVariables = customStepModel.Definition.CodeInputVariables,
            CodeOutputVariables = customStepModel.Definition.CodeOutputVariables
        };

        step.Setup(descriptor);

        return step;
    }

    private void LoadSteps()
    {        
        LoadAssemblySteps();

        LoadCustomSteps();
    }

    private void LoadAssemblySteps()
    {
        var assemblies = new[] { Assembly.Load(StepsAssembly) };

        var stepTypes = assemblies
            .SelectMany(x => x.GetAllWithBaseClass<StudioStep>())
            .Where(x => !x.IsAbstract && x != typeof(CustomStep));

        foreach (var stepType in stepTypes)
        {
            AddStep(stepType);
        }
    }

    private void LoadCustomSteps()
    {
        var categoryModel = CreateStepCategoryExplorerModel("Custom");
        explorerSteps.Add("Custom", categoryModel);

        var customSteps = Task.Run(customStepsService.List).Result;

        foreach (var customStep in customSteps)
        {
            var stepDescriptor = new CustomStepDescriptor
            {
                Name = customStep.Name,
                Type = nameof(CustomStep),
                DisplayName = customStep.DisplayName,
                Description = customStep.Description,
                MoreInfo = string.Empty,
                Category = "Custom",
                Definition = customStep.Definition,
                Icon = "code"
            };

            var explorerStep = CreateCustomStepExplorerModel(stepDescriptor);

            var category = explorerSteps[stepDescriptor.Category];
            category.Steps.Add(explorerStep);
        }
    }

    private void AddStep(Type stepType)
    {
        var stepDescriptor = stepTypeDescriptor.Describe(stepType);

        var explorerStep = CreateStepExplorerModel(stepDescriptor);

        solutionTypes.Add(explorerStep.Name, stepType);

        if (stepDescriptor.VisibleInExplorer)
        {
            if (string.IsNullOrEmpty(stepDescriptor.Category))
            {
                explorerSteps.Add(explorerStep.Name, explorerStep);
            }
            else
            {
                if (!explorerSteps.ContainsKey(stepDescriptor.Category))
                {
                    var categoryModel = CreateStepCategoryExplorerModel(stepDescriptor.Category);
                    explorerSteps.Add(stepDescriptor.Category, categoryModel);
                }

                var category = explorerSteps[stepDescriptor.Category];
                category.Steps.Add(explorerStep);
            }
        }
    }

    private static StepExplorerModel CreateStepExplorerModel(StepDescriptor stepDescriptor)
    {
        var stepExplorerModel = new StepExplorerModel
        {
            Name = stepDescriptor.Name,
            Type = stepDescriptor.Type,
            Description = stepDescriptor.Description,
            DisplayName = stepDescriptor.DisplayName,
            Category = stepDescriptor.Category,
            VisibleInExplorer = stepDescriptor.VisibleInExplorer,
            Icon = stepDescriptor.Icon
        };

        return stepExplorerModel;
    }

    private static CustomStepExplorerModel CreateCustomStepExplorerModel(CustomStepDescriptor stepDescriptor)
    {
        var stepExplorerModel = new CustomStepExplorerModel
        {
            Name = stepDescriptor.Name,
            Type = stepDescriptor.Type,
            Description = stepDescriptor.Description,
            DisplayName = stepDescriptor.DisplayName,
            Category = stepDescriptor.Category,
            VisibleInExplorer = stepDescriptor.VisibleInExplorer,
            Icon = stepDescriptor.Icon,
            Definition = stepDescriptor.Definition
        };

        return stepExplorerModel;
    }

    private static StepExplorerModel CreateStepCategoryExplorerModel(string category)
    {
        var stepExplorerModel = new StepExplorerModel
        {
            Name = category,
            Type = string.Empty,
            Description = category,
            DisplayName = category,
            Category = string.Empty,
            VisibleInExplorer = true,
            Icon = string.Empty
        };

        return stepExplorerModel;
    }
}
