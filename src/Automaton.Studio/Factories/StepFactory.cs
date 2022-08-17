using Automaton.Studio.Domain;
using Automaton.Studio.Domain.Interfaces;
using Automaton.Studio.Extensions;
using Automaton.Studio.Pages.Designer.Components.StepExplorer;
using System.Reflection;

namespace Automaton.Studio.Factories;

public class StepFactory
{
    private const string StepsAssembly = "Automaton.Studio";

    private IServiceProvider serviceProvider;
    private IDictionary<string, StepExplorerModel> solutionSteps;
    private IDictionary<string, Type> solutionTypes;
    private IStepTypeDescriptor stepTypeDescriptor;

    public StepFactory(IStepTypeDescriptor stepTypeDescriber, IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
        this.stepTypeDescriptor = stepTypeDescriber;
        solutionSteps = new Dictionary<string, StepExplorerModel>();
        solutionTypes = new Dictionary<string, Type>();

        var assembly = Assembly.Load(StepsAssembly);
        AddActivitiesFrom(assembly);
    }

    public IEnumerable<StepExplorerModel> GetSteps()
    {
        return solutionSteps.Values;
    }

    public void AddActivitiesFrom(Assembly assembly)
    {
        AddActivitiesFrom(new[] { assembly });
    }

    public void AddActivitiesFrom(IEnumerable<Assembly> assemblies)
    {
        var types = assemblies.SelectMany(x => x.GetAllWithBaseClass<StudioStep>()).Where(x => !x.IsAbstract);

        foreach (var type in types)
        {
            AddStep(type);
        }
    }

    public void AddStep(Type stepType)
    {
        var stepDescriptor = stepTypeDescriptor.Describe(stepType);

        var solutionStep = new StepExplorerModel 
        { 
            Name = stepDescriptor.Name, 
            Type = stepDescriptor.Type,
            Description = stepDescriptor.Description,
            DisplayName = stepDescriptor.DisplayName,
            Category = stepDescriptor.Category,
            Icon = stepDescriptor.Icon
        };

        solutionSteps.Add(solutionStep.Name, solutionStep);
        solutionTypes.Add(solutionStep.Name, stepType);
    }

    public StudioStep CreateStep(string name)
    {
        var descriptor = stepTypeDescriptor.Describe(solutionTypes[name]);

        var step = serviceProvider.GetService(solutionTypes[name]) as StudioStep;

        step.Setup(descriptor);

        return step;
    }
}
