using Automaton.Studio.Attributes;
using Automaton.Studio.Domain;

namespace Automaton.Studio.Steps.TestAssert;

[StepDescription(
    Name = "TestAssert",
    Type = "TestAssert",
    DisplayName = "Test assert",
    Category = "Test",
    Description = "Evaluate conditional expression and record its result",
    MoreInfo = "https://www.automaton.studio/documentation",
    Icon = "schedule"
)]
public class TestAssertStep : StudioStep
{
    public string Expression
    {
        get => GetInputValue<string>(nameof(Expression));
        set => SetInputValue(nameof(Expression), value);
    }

    public string Error { get; set; }

    public TestAssertStep()
    {
        SetInputValue(nameof(Expression), string.Empty);
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
