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
    Description = "Generates a report based on all the flow tests",
    MoreInfo = "https://www.automaton.studio/documentation/testreportstep",
    Icon = "profile"
)]
public class TestReportStep : StudioStep
{
    private const string ReportVariableName = "TestReport";

    public int TotalTests { get; set; }

    public int SuccessfulTests { get; set; }

    public int FailedTests { get; set; }

    public string Report { get; set; }

    public StepVariable ReportVariable { get; set; }

    public TestReportStep()
    {
        Created += OnCreated;
    }

    public override Type GetDesignerComponent()
    {
        return typeof(TestReportDesigner);
    }

    public override Type GetPropertiesComponent()
    {
        return typeof(TestReportProperties);
    }

    private void OnCreated(object sender, StepEventArgs e)
    {
        ReportVariable = new StepVariable
        {
            OldName = ReportVariableName,
            Name = $"{ReportVariableName}{Flow.GetNumberOfSteps<TestReportStep>()}",
        };

        SetVariable(ReportVariable);
    }
}
