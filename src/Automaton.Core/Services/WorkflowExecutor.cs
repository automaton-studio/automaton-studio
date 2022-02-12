using Automaton.Core.Interfaces;
using Automaton.Core.Models;
using Microsoft.Extensions.Logging;

namespace Automaton.Core.Services
{
    public class WorkflowExecutor : IWorkflowExecutor
    {
        protected readonly ILogger _logger;
        protected readonly IServiceProvider _serviceProvider;

        public WorkflowExecutor(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
        {
            _serviceProvider = serviceProvider;
            _logger = loggerFactory.CreateLogger<WorkflowExecutor>();
        }

        public async Task<WorkflowExecutorResult> Execute(Workflow workflow, CancellationToken cancellationToken = default)
        {
            var result = new WorkflowExecutorResult();

            var definition = workflow.GetStartupDefinition();

            var step = definition.GetFirstStep();

            while (step != null)
            {
                try
                {
                    await ExecuteStep(definition, step, result, definition, cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Workflow {0} raised error on step {1} Message: {2}", definition.Id, step.Id, ex.Message);
                    result.Errors.Add(new ExecutionError
                    {
                        WorkflowId = definition.Id,
                        ErrorTime = DateTime.UtcNow,
                        Message = ex.Message
                    });
                }

                step = step.NextStepId != null ? definition.StepsDictionary[step.NextStepId] : null;
            }

            return result;
        }

        private async Task ExecuteStep(WorkflowDefinition workflow, WorkflowStep step, WorkflowExecutorResult wfResult, WorkflowDefinition def, CancellationToken cancellationToken = default)
        {
            var context = new StepExecutionContext
            {
                WorkflowDefinition = workflow,
                Step = step,
                CancellationToken = cancellationToken
            };

            _logger.LogDebug("Starting step {0} on workflow {1}", step.Name, workflow.Id);

            await step.RunAsync(context);
        }
    }
}
