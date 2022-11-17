using Automaton.Core.Models;
using Automaton.Studio.Attributes;
using Automaton.Studio.Domain;
using Automaton.Studio.Events;

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
    private const string ReportVariableName = "TestReport";

    public int TotalTests { get; set; }

    public int PassedTests { get; set; }

    public int FailedTests { get; set; }

    public string Report { get; set; }

    public TestReportStep()
    {
        Finalize += OnFinalize;
    }

    public override Type GetDesignerComponent()
    {
        return typeof(TestReportDesigner);
    }

    public override Type GetPropertiesComponent()
    {
        return typeof(TestReportProperties);
    }

    private void OnFinalize(object sender, StepEventArgs e)
    {
        var reportVariable = new StepVariable
        {
            Key = ReportVariableName,
            Name = $"{ReportVariableName}{Flow.GetNumberOfSteps<TestReportStep>()}",
        };

        SetOutputVariable(reportVariable);
    }
}
