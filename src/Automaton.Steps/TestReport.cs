using Automaton.Core.Models;
using System.Text;

namespace Automaton.Steps;

public class TestReport : WorkflowStep
{
    private const string ReportVariableName = "TestReport";

    public int TotalTests { get; set; }

    public int PassedTests { get; set; }

    public int FailedTests { get; set; }

    public string Report { get; set; }

    protected override Task<ExecutionResult> RunAsync(StepExecutionContext context)
    {
        var tests = context.Workflow.Definitions.SelectMany(x => x.Steps.Select(x => x.Value).Where(x => x is Test)) as IEnumerable<Test>;

        var report = new StringBuilder();
        report.AppendLine($"Total tests: {tests.Count()}");
        report.AppendLine($"Passed tests: {tests.Where(x => !x.Errors.Any())}");
        report.AppendLine($"Failed tests: {tests.Where(x => x.Errors.Any())}");

        var reportVariable = Outputs[ReportVariableName];
        reportVariable.Value = report;

        return Task.FromResult(ExecutionResult.Next());
    }
}
