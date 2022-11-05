using Automaton.Studio.Attributes;
using Automaton.Studio.Domain;
using Automaton.Studio.Events;
using Automaton.Studio.Factories;
using System.Reflection;

namespace Automaton.Studio.Steps.Sequence;

[StepDescription(
    Name = "Sequence",
    Type = "Sequence",
    DisplayName = "Sequence",
    Category = "Sequence",
    Description = "Sequence",
    Icon = "code"
)]
public class SequenceStep : StudioStep
{
    private readonly StepFactory stepFactory;

    public override bool HasProperties { get; set; } = false;

    public SequenceEndStep SequenceEndStep { get; set; }

    public IList<StudioStep> SequenceSteps { get; set; } = new List<StudioStep>();

    public SequenceStep(StepFactory stepFactory)
    {
        this.stepFactory = stepFactory;

        Finalize += OnFinalize;
    }

    public override Type GetDesignerComponent()
    {
        return typeof(SequenceDesigner);
    }

    public override Type GetPropertiesComponent()
    {
        throw new NotImplementedException();
    }

    public override void Select()
    {
        base.SetSelectClass();

        if (!IsFinal())
            return;

        var sequenceStepIndex = Definition.Steps.IndexOf(this);
        var endSequenceStepIndex = Definition.Steps.IndexOf(SequenceEndStep);
        var count = endSequenceStepIndex - sequenceStepIndex;
        var children = Definition.Steps.GetRange(sequenceStepIndex + 1, count);

        foreach(var child in children)
        {
            child.SetSelectClass();
        }
    }

    private void OnFinalize(object sender, StepEventArgs e)
    {
        var sequenceEndStep = CreateSequenceEndStep();
        AddSequenceEndStep(sequenceEndStep);
    }

    private void AddSequenceEndStep(SequenceEndStep sequenceEndStep)
    {
        SequenceEndStep = sequenceEndStep;
        Definition.Steps.Insert(Definition.Steps.IndexOf(this) + 1, sequenceEndStep);
        Definition.FinalizeStep(sequenceEndStep);
    }

    private SequenceEndStep CreateSequenceEndStep()
    {
        var stepDescription = typeof(SequenceEndStep).GetCustomAttribute<StepDescriptionAttribute>(false);
        var sequenceEndStep = stepFactory.CreateStep(stepDescription?.Name) as SequenceEndStep;
        sequenceEndStep.Definition = Definition;
        sequenceEndStep.SequenceStep = this;

        return sequenceEndStep;
    }
}
