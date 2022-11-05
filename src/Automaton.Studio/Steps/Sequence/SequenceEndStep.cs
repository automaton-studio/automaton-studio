using Automaton.Studio.Attributes;
using Automaton.Studio.Domain;

namespace Automaton.Studio.Steps.Sequence;

[StepDescription(
    Name = "SequenceEnd",
    Type = "SequenceEnd",
    DisplayName = "Sequence End",
    Category = "Sequence",
    Description = "Sequence End",
    Icon = "code"
)]
public class SequenceEndStep : StudioStep
{
    public override bool HasProperties { get; set; } = false;

    public SequenceStep SequenceStep { get; set; }

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
}
