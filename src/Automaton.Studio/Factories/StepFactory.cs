using Automaton.Core.Models;
using Automaton.Studio.Domain;
using Automaton.Studio.Extensions;
using Automaton.Studio.Pages.FlowDesigner.Components.StepExplorer;
using Automaton.Studio.Services;
using System.Reflection;
using System.Threading.Tasks;

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

    public StudioStep CreateStep(Step step)
    {
        var studioStep = serviceProvider.GetService(solutionTypes[step.Type]) as StudioStep;

        studioStep.Setup(step);

        return studioStep;
    }

    public StudioStep CreateCustomStep(CustomStepExplorerModel customStepModel)
    {
        var step = new Steps.Custom.CustomStep
        {
            Code = customStepModel.Definition.Code,
            CodeInputVariables = customStepModel.Definition.CodeInputVariables,
            CodeOutputVariables = customStepModel.Definition.CodeOutputVariables
        };

        step.Setup(new StepDescriptor
        {
            Name = customStepModel.Name,
            Type = customStepModel.Type,
            DisplayName = customStepModel.DisplayName,
            Description = customStepModel.Description,
            Category = customStepModel.Category,
            Icon = customStepModel.Icon,
            MoreInfo = customStepModel.MoreInfo,
            VisibleInExplorer = customStepModel.VisibleInExplorer
        });

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
        var customSteps = Task.Run(customStepsService.List).Result;

        foreach (var customStep in customSteps)
        {
            AddExplorerCategory(customStep.Category);

            var explorerStep = CreateCustomStepExplorerModel(customStep);

            var category = explorerSteps[customStep.Category];
            category.Steps.Add(explorerStep);
        }
    }

    private void AddExplorerCategory(string categoryName)
    {
        if (!explorerSteps.ContainsKey(categoryName))
        {
            var categoryModel = CreateCategoryModel(categoryName);
            explorerSteps.Add(categoryName, categoryModel);
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
                    var categoryModel = CreateCategoryModel(stepDescriptor.Category);
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

    private static CustomStepExplorerModel CreateCustomStepExplorerModel(CustomStep customStep)
    {
        var stepExplorerModel = new CustomStepExplorerModel
        {
            Name = customStep.Name,
            Type = customStep.Type,
            Description = customStep.Description,
            DisplayName = customStep.DisplayName,
            Category = customStep.Category,
            VisibleInExplorer = customStep.VisibleInExplorer,
            Icon = customStep.Icon,
            Definition = customStep.Definition
        };

        return stepExplorerModel;
    }

    private static StepExplorerModel CreateCategoryModel(string category)
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
