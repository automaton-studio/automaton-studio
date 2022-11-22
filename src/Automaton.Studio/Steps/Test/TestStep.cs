using Automaton.Studio.Attributes;
using Automaton.Studio.Factories;
using Automaton.Studio.Steps.Sequence;

namespace Automaton.Studio.Steps.Test;

[StepDescription(
    Name = "Test",
    Type = "Test",
    DisplayName = "Test",
    Category = "Test",
    Description = "Test",
    Icon = "schedule"
)]
public class TestStep : SequenceStep
{    
    public IList<string> Errors { get; set; } = new List<string>();

    public TestStep(StepFactory stepFactory) 
        : base(stepFactory)
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
