using AntDesign;
using Automaton.Core.Models;
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
    Category = "Flow control",
    Description = "Container used to group steps together",
    Icon = "block"
)]
public class SequenceStep : StudioStep
{
    private readonly StepFactory stepFactory;

    protected override string StepClass { get; set; } = "designer-sequence-step";
    protected override string SelectedStepClass { get; set; } = "designer-sequence-step-selected";
    protected override string DisabledStepClass { get; set; } = "designer-sequence-step-disabled";

    public bool Collapsed { get; set; }

    public SequenceEndStep SequenceEndStep 
    {
        get
        {
            var sequenceEnd = Definition.Steps.SingleOrDefault(x => x.Id == SequenceEndStepId);
            return sequenceEnd as SequenceEndStep;
        }
    }

    public string SequenceEndStepId
    {
        get => GetInputValue(nameof(SequenceEndStepId)) as string;
        set => SetInputValue(nameof(SequenceEndStepId), value);
    }

    public IList<StudioStep> SequenceSteps { get; set; } = new List<StudioStep>();

    public SequenceStep(StepFactory stepFactory)
    {
        this.stepFactory = stepFactory;
        HasProperties = false;
    }

    public void Delete()
    {
        var sequenceStepIndex = Definition.Steps.IndexOf(this);
        var endSequenceStepIndex = Definition.Steps.IndexOf(SequenceEndStep);
        var count = endSequenceStepIndex - sequenceStepIndex;

        Definition.DeleteSteps(sequenceStepIndex, count + 1);
    }

    public override Type GetDesignerComponent()
    {
        return typeof(SequenceDesigner);
    }

    public override Type GetPropertiesComponent()
    {
        throw new NotImplementedException();
    }

    public override void Complete()
    {
        base.Complete();

        var sequenceEndStep = CreateSequenceEndStep();

        AddEndSequenceStep(sequenceEndStep);
    }

    public override void Select()
    {
        base.Select();

        var childrenPlusEndStep = GetChildrenIncludingEndStep();

        foreach (var child in childrenPlusEndStep)
        {
            child.Select();
        }
    }

    public void SelectExcludingEndStep()
    {
        base.Select();

        var childrenPlusEndStep = GetChildrenExcludingEndStep();

        foreach (var child in childrenPlusEndStep)
        {
            child.Select();
        }
    }

    public IEnumerable<StudioStep> GetChildrenExcludingEndStep()
    {
        var sequenceStepIndex = Definition.Steps.IndexOf(this);
        var endSequenceStepIndex = Definition.Steps.IndexOf(SequenceEndStep);
        var count = endSequenceStepIndex - sequenceStepIndex;
        var children = Definition.Steps.GetRange(sequenceStepIndex + 1, count - 1);

        return children;
    }

    public IEnumerable<StudioStep> GetChildrenIncludingEndStep()
    {
        var sequenceStepIndex = Definition.Steps.IndexOf(this);
        var endSequenceStepIndex = Definition.Steps.IndexOf(SequenceEndStep);
        var count = endSequenceStepIndex - sequenceStepIndex;
        var children = Definition.Steps.GetRange(sequenceStepIndex + 1, count);

        return children;
    }

    private void AddEndSequenceStep(SequenceEndStep sequenceEndStep)
    {
        // Start and end sequences must know about each other
        SequenceEndStepId = sequenceEndStep.Id;

        // Add sequence end to definition's list of steps
        Definition.Steps.Insert(Definition.Steps.IndexOf(this) + 1, sequenceEndStep);

        // Ask definition to finalize sequence end
        Definition.CompleteStep(sequenceEndStep);
    }

    /// <summary>
    /// Create a sequence end.
    /// </summary>
    /// <returns></returns>
    private SequenceEndStep CreateSequenceEndStep()
    {
        var stepDescription = typeof(SequenceEndStep).GetCustomAttribute<StepDescriptionAttribute>(false);
        
        var sequenceEndStep = stepFactory.CreateStep(stepDescription?.Name, Definition) as SequenceEndStep;
        sequenceEndStep.SequenceStepId = Id;
        sequenceEndStep.ParentId = ParentId;

        return sequenceEndStep;
    }
}
