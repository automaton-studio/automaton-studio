using AutoMapper;
using Automaton.Studio.Domain;
using Automaton.Studio.Events;
using Automaton.Studio.Factories;
using Automaton.Studio.Pages.Designer.Components.StepExplorer;

namespace Automaton.Studio.Steps.Sequence;

public class SequenceStepDesignerViewModel
{
    private readonly IMapper mapper;
    private readonly StepFactory stepFactory;

    public List<StudioStep> Steps { get; set; } = new List<StudioStep>();

    public event EventHandler<StepEventArgs> DragStep;
    public event EventHandler<StepEventArgs> StepAdded;
    public event EventHandler<StepEventArgs> StepRemoved;

    public SequenceStepDesignerViewModel
    (
        IMapper mapper,
        StepFactory stepFactory
    )
    {
        this.mapper = mapper;
        this.stepFactory = stepFactory;
    }

    public void CreateStep(StepExplorerModel solutionStep)
    {
        var step = stepFactory.CreateStep(solutionStep.Name);

        DragStep?.Invoke(this, new StepEventArgs(step));
    }

    public IEnumerable<StudioStep> GetSelectedSteps()
    {
        return Steps.Where(x => x.IsSelected());
    }

    public void DeleteStep(StudioStep step)
    {
        Steps.Remove(step);

        UpdateStepConnections();

        StepRemoved?.Invoke(this, new StepEventArgs(step));
    }

    public void FinalizeStep(StudioStep step)
    {
        step.MarkAsFinal();

        // TODO! ViewMOdel should know about current Definition
        //step.Definition = this;

        UpdateStepConnections();

        StepAdded?.Invoke(this, new StepEventArgs(step));
    }

    public void UpdateStepConnections()
    {
        for (var i = 0; i < Steps.Count; i++)
        {
            Steps[i].NextStepId = i != Steps.Count - 1 ? Steps[i + 1].Id : null;
        }
    }
}
