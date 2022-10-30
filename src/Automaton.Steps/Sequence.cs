using Automaton.Core.Models;
using Microsoft.Extensions.Logging;

namespace Automaton.Steps;

public class Sequence : WorkflowStep
{
    private readonly ILoggerFactory _loggerFactory;

    public LogLevel Level { get; set; } = LogLevel.Information;

    public Sequence(ILoggerFactory loggerFactory)
    {
        _loggerFactory = loggerFactory;
    }

    protected override Task<ExecutionResult> RunAsync(StepExecutionContext context)
    {
        var logger = _loggerFactory.CreateLogger(context.Definition.Id);

        return Task.FromResult(ExecutionResult.Next());
    }
}
