using Automaton.Core.Models;
using System.Text;

namespace Automaton.Steps;

public class TestReport : WorkflowStep
{
    private const string ReportVariableName = "TestReport";

    public int TotalTests { get; set; }

    public int SuccessfulTests { get; set; }

    public int FailedTests { get; set; }

    public string? Report { get; set; }

    protected override Task<ExecutionResult> RunAsync(StepExecutionContext context)
    {
        var tests = context.Workflow.Definitions
            .SelectMany(x => x.Steps.Select(x => x.Value).Where(x => x is Test))
            .Select(x => x as Test);

        TotalTests = tests.Count();
        SuccessfulTests = tests.Count(x => !x.Errors.Any());
        FailedTests = tests.Count(x => x.Errors.Any());

        SetOutputVariable(ReportVariableName, GetReport(tests), context.Workflow);

        return Task.FromResult(ExecutionResult.Next());
    }

    private string GetReport(IEnumerable<Test> tests)
    {
        var report = new StringBuilder();

        report.AppendLine($"Total tests: {TotalTests} Successful: {SuccessfulTests} Failed: {FailedTests}");
        report.Append($"{GetSuccessfulTestsReport(tests)}");
        report.Append($"{GetFailedTestsReport(tests)}");

        return report.ToString();
    }

    private static string GetSuccessfulTestsReport(IEnumerable<Test> tests)
    {
        var report = new StringBuilder();

        var successfulTests = tests.Where(x => !x.Errors.Any());

        if (successfulTests.Any())
        {
            report.AppendLine($"Successful tests");
        }

        foreach (var test in successfulTests)
        {
            report.AppendLine($"Test {test.Name} succeeded.");
        }

        return report.ToString();
    }

    private static string GetFailedTestsReport(IEnumerable<Test> tests)
    {
        var report = new StringBuilder();

        var failedTests = tests.Where(x => x.Errors.Any());

        if (failedTests.Any())
        {
            report.AppendLine($"Failed tests");
        }

        foreach (var test in failedTests)
        {
            report.AppendLine($"Test {test.Name} failed with the error:");

            foreach (var error in test.Errors)
            {
                report.AppendLine($"{error}");
            }
        }

        return report.ToString();
    }
}
