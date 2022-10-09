using Automaton.Studio.Attributes;
using Automaton.Studio.Domain;

namespace Automaton.Studio.Steps.Test;

[StepDescription(
    Name = "Test",
    Type = "Test",
    DisplayName = "Test",
    Category = "Test",
    Description = "Test",
    Icon = "code"
)]
public class TestStep : StudioStep
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

    public TestStep()
    {
    }

    public override Type GetDesignerComponent()
    {
        return typeof(TestDesigner);
    }

    public override Type GetPropertiesComponent()
    {
        return typeof(TestProperties);
    }
}
