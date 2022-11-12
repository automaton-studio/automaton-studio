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
    public string Description
    {
        get
        {
            return Inputs.ContainsKey(nameof(Description)) ?
                Inputs[nameof(Description)].ToString() : string.Empty;
        }

        set
        {
            Inputs[nameof(Description)] = value;
        }
    }

    public IList<string> Errors { get; set; }

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
