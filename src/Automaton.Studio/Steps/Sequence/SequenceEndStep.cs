using Automaton.Studio.Attributes;
using Automaton.Studio.Domain;

namespace Automaton.Studio.Steps.Sequence;

[StepDescription(
    Name = "SequenceEnd",
    Type = "SequenceEnd",
    DisplayName = "End",
    Category = "Flow control",
    Description = "Ends a flow control",
    VisibleInExplorer = false,
    Icon = "flag"
)]
public class SequenceEndStep : StudioStep
{
    protected override string StepClass { get; set; } = "designer-sequence-end-step";
    protected override string SelectedStepClass { get; set; } = "designer-sequence-end-step-selected";
    protected override string DisabledStepClass { get; set; } = "designer-sequence-end-step-disabled";

    public bool Collapsed { get; set; }

    public SequenceStep SequenceStep
    {
        get
        {
            var sequenceStep = Definition.Steps.SingleOrDefault(x => x.Id == SequenceStepId);

            return sequenceStep as SequenceStep;
        }
    }

    public string SequenceStepId
    {
        get => GetInputValue<string>(nameof(SequenceStepId));
        set => SetInputValue(nameof(SequenceStepId), value);
    }

    public SequenceEndStep()
    {
        HasProperties = false;
    }

    public override Type GetDesignerComponent()
    {
        return typeof(SequenceEndDesigner);
    }

    public override Type GetPropertiesComponent()
    {
        throw new NotImplementedException();
    }

    public void Delete()
    {
        var sequenceStepIndex = Definition.Steps.IndexOf(SequenceStep);
        var endSequenceStepIndex = Definition.Steps.IndexOf(this);
        var count = endSequenceStepIndex - sequenceStepIndex;

        Definition.DeleteSteps(sequenceStepIndex, count + 1);
    }

    public override void Select()
    {
        base.Select();

        SequenceStep.SelectExcludingEndStep();
    }

    public override void UpdateParent()
    {
        ParentId = SequenceStep.ParentId;
    }
}
