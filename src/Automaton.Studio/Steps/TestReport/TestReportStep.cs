using Automaton.Studio.Attributes;
using Automaton.Studio.Domain;

namespace Automaton.Studio.Steps.TestReport;

[StepDescription(
    Name = "TestReport",
    Type = "TestReport",
    DisplayName = "Test report",
    Category = "Test",
    Description = "Test report",
    Icon = "profile"
)]
public class TestReportStep : StudioStep
{
    public string Expression
    {
        get => Inputs.ContainsKey(nameof(Expression)) ?
               Inputs[nameof(Expression)]?.ToString() : string.Empty;
        set => Inputs[nameof(Expression)] = value;
    }

    public string Error { get; set; }

    public TestReportStep()
    {
    }

    public override Type GetDesignerComponent()
    {
        return typeof(TestReportDesigner);
    }

    public override Type GetPropertiesComponent()
    {
        return typeof(TestReportProperties);
    }
}
