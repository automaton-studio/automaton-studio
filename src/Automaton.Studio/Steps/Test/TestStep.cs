using Automaton.Studio.Attributes;
using Automaton.Studio.Domain;
using Automaton.Studio.Steps.Sequence;

namespace Automaton.Studio.Steps.Test;

[StepDescription(
    Name = "Test",
    Type = "Test",
    DisplayName = "Test",
    Category = "Test",
    Description = "Test",
    Icon = "code"
)]
public class TestStep : SequenceStep
{
    public TestStep()
    {
    }

    public override Type GetDesignerComponent()
    {
        return typeof(TestDesigner);
    }

    public override Type GetPropertiesComponent()
    {
        throw new NotImplementedException();
    }
}
