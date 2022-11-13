using Automaton.Studio.Domain;
using Automaton.Studio.Domain.Interfaces;
using Automaton.Studio.Extensions;
using Automaton.Studio.Pages.Designer.Components.StepExplorer;
using System.Reflection;

namespace Automaton.Studio.Factories;

public class StepFactory
{
    private const string StepsAssembly = "Automaton.Studio";

    private readonly IServiceProvider serviceProvider;
    private readonly IDictionary<string, StepExplorerModel> solutionSteps;
    private readonly IDictionary<string, Type> solutionTypes;
    private readonly IStepTypeDescriptor stepTypeDescriptor;

    public StepFactory(IStepTypeDescriptor stepTypeDescriptor, IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
        this.stepTypeDescriptor = stepTypeDescriptor;
        solutionSteps = new Dictionary<string, StepExplorerModel>();
        solutionTypes = new Dictionary<string, Type>();

        AddStepsFrom(Assembly.Load(StepsAssembly));
    }

    public IEnumerable<StepExplorerModel> GetSteps()
    {
        return solutionSteps.Values;
    }

    public void AddStepsFrom(Assembly assembly)
    {
        AddStepsFrom(new[] { assembly });
    }

    public void AddStepsFrom(IEnumerable<Assembly> assemblies)
    {
        var stepTypes = assemblies.SelectMany(x => x.GetAllWithBaseClass<StudioStep>()).Where(x => !x.IsAbstract);

        foreach (var stepType in stepTypes)
        {
            AddStep(stepType);
        }
    }

    public void AddStep(Type stepType)
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

    public StudioStep CreateStep(string name)
    {
        var descriptor = stepTypeDescriptor.Describe(solutionTypes[name]);
        var step = serviceProvider.GetService(solutionTypes[name]) as StudioStep;

        step.Setup(descriptor);

        return step;
    }

    private StepExplorerModel CreateStepExplorerModel(IStepDescriptor stepDescriptor)
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

    private StepExplorerModel CreateStepCategoryExplorerModel(string category)
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
