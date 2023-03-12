using Automaton.Studio.Domain;
using Automaton.Studio.Domain.Interfaces;
using Automaton.Studio.Extensions;
using Automaton.Studio.Pages.FlowDesigner.Components.StepExplorer;
using System.Reflection;

namespace Automaton.Studio.Factories;

public class StepFactory
{
    private const string StepsAssembly = "Automaton.Studio";

    private readonly IServiceProvider serviceProvider;
    private readonly IStepTypeDescriptor stepTypeDescriptor;
    private readonly IDictionary<string, StepExplorerModel> solutionSteps = new Dictionary<string, StepExplorerModel>();
    private readonly IDictionary<string, Type> solutionTypes = new Dictionary<string, Type>();

    public StepFactory(IStepTypeDescriptor stepTypeDescriptor, IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
        this.stepTypeDescriptor = stepTypeDescriptor;

        LoadSteps();
    }

    public IEnumerable<StepExplorerModel> GetSteps()
    {
        return solutionSteps.Values;
    }

    public StudioStep CreateStep(string name, bool isFinal = false)
    {
        var descriptor = stepTypeDescriptor.Describe(solutionTypes[name]);
        var step = serviceProvider.GetService(solutionTypes[name]) as StudioStep;

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
        // TODO!
        // Load custom steps from database
    }

    private void AddStep(Type stepType)
    {
        var stepDescriptor = stepTypeDescriptor.Describe(stepType);

        var solutionStep = CreateStepExplorerModel(stepDescriptor);

        solutionTypes.Add(solutionStep.Name, stepType);

        if (stepDescriptor.VisibleInExplorer)
        {
            if (string.IsNullOrEmpty(stepDescriptor.Category))
            {
                solutionSteps.Add(solutionStep.Name, solutionStep);
            }
            else
            {
                if (!solutionSteps.ContainsKey(stepDescriptor.Category))
                {
                    var categoryModel = CreateStepCategoryExplorerModel(stepDescriptor.Category);
                    solutionSteps.Add(stepDescriptor.Category, categoryModel);
                }

                var category = solutionSteps[stepDescriptor.Category];
                category.Steps.Add(solutionStep);
            }
        }
    }

    private static StepExplorerModel CreateStepExplorerModel(IStepDescriptor stepDescriptor)
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
