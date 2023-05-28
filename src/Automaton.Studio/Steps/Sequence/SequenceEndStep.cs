using Automaton.Studio.Attributes;
using Automaton.Studio.Domain;

namespace Automaton.Studio.Steps.Sequence;

[StepDescription(
    Name = "SequenceEnd",
    Type = "SequenceEnd",
    DisplayName = "End sequence",
    Category = "Flow control",
    Description = "End sequence",
    VisibleInExplorer = false,
    Icon = "flag"
)]
public class SequenceEndStep : StudioStep
{
    public override bool HasProperties { get; set; } = false;

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
        get => GetInputValue(nameof(SequenceStepId)) as string;
        set => SetInputValue(nameof(SequenceStepId), value);
    }

    public override Type GetDesignerComponent()
    {
        return typeof(SequenceEndDesigner);
    }

    public override Type GetPropertiesComponent()
    {
        throw new NotImplementedException();
    }

    public override void Select()
    {
        SequenceStep.Select();
    }

    public override void Setup(StepDescriptor descriptor)
    {
        base.Setup(descriptor);

        SetupStepClass();
    }

    private void SetupStepClass()
    {
        StepClass = "designer-sequence-end-step";
        SelectedStepClass = "designer-sequence-end-step-selected";
        DisabledStepClass = "designer-sequence-end-step-disabled";

        Class = StepClass;
    }
}
