using Automaton.Core.Models;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

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
        var tests = context.Workflow.Definitions
            .SelectMany(x => x.Steps.Select(x => x.Value).Where(x => x is Test))
            .Select(x => x as Test);

        TotalTests = tests.Count();
        PassedTests = tests.Count(x => !x.Errors.Any());
        FailedTests = tests.Count(x => x.Errors.Any());

        SetOutputVariable(ReportVariableName, GetReport(tests), context.Workflow);

        return Task.FromResult(ExecutionResult.Next());
    }

    private string GetReport(IEnumerable<Test> tests)
    {
        var report = new StringBuilder();
        report.AppendLine($"Total tests: {TotalTests}");
        report.AppendLine($"Passed tests: {PassedTests}");
        report.AppendLine($"{GetPassedTestsReport(tests)}");
        report.AppendLine($"Failed tests: {FailedTests}");
        report.AppendLine($"{GetFailedTestsReport(tests)}");

        return report.ToString();
    }

    private string GetPassedTestsReport(IEnumerable<Test> tests)
    {
        var report = new StringBuilder();

        var passedTests = tests.Where(x => !x.Errors.Any());

        foreach (var test in passedTests)
        {
            report.AppendLine($"Test {test.Name} succeeded.");

            report.AppendLine();
        }

        return report.ToString();
    }

    private string GetFailedTestsReport(IEnumerable<Test> tests)
    {
        var report = new StringBuilder();

        var failedTests = tests.Where(x => x.Errors.Any());

        foreach (var test in failedTests)
        {
            report.AppendLine($"Test {test.Name} failed with the errors:");

            foreach (var error in test.Errors)
            {
                report.AppendLine($"{error}");
            }

            report.AppendLine();
        }

        return report.ToString();
    }
}
