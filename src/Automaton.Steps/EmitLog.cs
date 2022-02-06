using Automaton.Core.Interfaces;
using Automaton.Core.Models;
using Microsoft.Extensions.Logging;

namespace Automaton.Steps
{
    public class EmitLog : StepBodyAsync
    {
        private readonly ILoggerFactory _loggerFactory;

        public string Message { get; set; }

        public LogLevel Level { get; set; } = LogLevel.Information;

        public EmitLog(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }

        public override Task<ExecutionResult> RunAsync(IStepExecutionContext context)
        {
            var logger = _loggerFactory.CreateLogger(context.WorkflowDefinition.Id);
            logger.Log(Level, default(EventId), Message, null, (state, ex) => state);
            return Task.FromResult(ExecutionResult.Next());
        }
    }
}
