using Automaton.Studio.Attributes;
using Automaton.Studio.Domain;
using Automaton.Studio.Domain.Interfaces;

namespace Automaton.Studio.Steps.Sequence;

[StepDescription(
    Name = "SequenceEnd",
    Type = "SequenceEnd",
    DisplayName = "End",
    Category = "Sequence",
    Description = "Sequence End",
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
        get => GetInputVariable(nameof(SequenceStepId)) as string;
        set => SetInputVariable(nameof(SequenceStepId), value);
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

    public override void Setup(IStepDescriptor descriptor)
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
