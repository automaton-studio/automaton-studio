using Automaton.Studio.Attributes;
using Automaton.Studio.Domain;

namespace Automaton.Studio.Steps.TestAssert;

[StepDescription(
    Name = "TestAssert",
    Type = "TestAssert",
    DisplayName = "Test assert",
    Category = "Test",
    Description = "Test assertions",
    Icon = "schedule"
)]
public class TestAssertStep : StudioStep
{
    public string Expression
    {
        get => Inputs.ContainsKey(nameof(Expression)) ?
               Inputs[nameof(Expression)]?.ToString() : string.Empty;
        set => Inputs[nameof(Expression)] = value;
    }

    public string Error { get; set; }

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
