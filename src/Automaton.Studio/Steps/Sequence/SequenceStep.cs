using Automaton.Studio.Attributes;
using Automaton.Studio.Domain;

namespace Automaton.Studio.Steps.Sequence
{

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
        public override bool HasProperties { get; set; } = false;

        public override Type GetDesignerComponent()
        {
            return typeof(SequenceDesigner);
        }

        public override Type GetPropertiesComponent()
        {
            throw new NotImplementedException();
        }
    }
}
