﻿using Automaton.Core.Models;
using Automaton.Studio.Attributes;
using Automaton.Studio.Domain;
using Automaton.Studio.Events;
using Automaton.Studio.Resources;

namespace Automaton.Studio.Steps.TestReport;

[StepDescription(
    Name = "TestReport",
    Type = "TestReport",
    DisplayName = "Test report",
    Category = "Test",
    Description = "Generates a report based on flow tests",
    MoreInfo = "https://www.automaton.studio/documentation",
    Icon = "profile"
)]
public class TestReportStep : StudioStep
{
    private const string ReportVariableKey = "TestReport";

    public int TotalTests { get; set; }

    public int SuccessfulTests { get; set; }

    public int FailedTests { get; set; }

    public string Report { get; set; }
     
    public override Type GetDesignerComponent()
    {
        return typeof(TestReportDesigner);
    }

    public override Type GetPropertiesComponent()
    {
        return typeof(TestReportProperties);
    }

    public override void Created()
    {
        base.Created();

        var reportVariable = new StepVariable
        {
            Id = Guid.NewGuid().ToString(),
            Name = $"{ReportVariableKey}{Flow.GetNumberOfSteps<TestReportStep>()}",
            Description = Variables.TestReport
        };

        SetOutputVariable(reportVariable);
    }
}
