using Automaton.Studio.Attributes;
using Automaton.Studio.Domain;

namespace Automaton.Studio.Steps.TestAssert;

[StepDescription(
    Name = "TestAssert",
    Type = "TestAssert",
    DisplayName = "Test assert",
    Category = "Console",
    Description = "Test assertions",
    Icon = "schedule"
)]
public class TestAssertStep : StudioStep
{
    public string Message
    {
        get
        {
            return Inputs.ContainsKey(nameof(Message)) ?
                Inputs[nameof(Message)].ToString() : string.Empty;
        }

        set
        {
            Inputs[nameof(Message)] = value;
        }
    }

    public TestAssertStep()
    {
    }

    public override Type GetDesignerComponent()
    {
        return typeof(TestAssertDesigner);
    }

    public override Type GetPropertiesComponent()
    {
        return typeof(TestAssertProperties);
    }
}
