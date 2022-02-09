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

        public async Task<WorkflowExecutorResult> Execute(WorkflowDefinition workflowDefinition, CancellationToken cancellationToken = default)
        {
            var wfResult = new WorkflowExecutorResult();

            foreach (WorkflowStep step in workflowDefinition.Steps)
            {
                try
                {
                    await ExecuteStep(workflowDefinition, step, wfResult, workflowDefinition, cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Workflow {0} raised error on step {1} Message: {2}", workflowDefinition.Id, step.Id, ex.Message);
                    wfResult.Errors.Add(new ExecutionError
                    {
                        WorkflowId = workflowDefinition.Id,
                        ErrorTime = DateTime.UtcNow,
                        Message = ex.Message
                    });
                }
            }

            return wfResult;
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
