﻿using Automaton.Core.Models;
using Microsoft.Extensions.Logging;

namespace Automaton.Core.Services
{
    public class WorkflowExecuteService
    {
        protected readonly ILogger logger;
        protected readonly IServiceProvider serviceProvider;
        private readonly FlowConvertService flowConvertService;

        public WorkflowExecuteService(IServiceProvider serviceProvider, FlowConvertService flowConvertService, ILoggerFactory loggerFactory)
        {
            this.serviceProvider = serviceProvider;
            this.flowConvertService = flowConvertService;
            logger = loggerFactory.CreateLogger<WorkflowExecuteService>();
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
                    var context = new StepExecutionContext
                    {
                        Workflow = workflow,
                        Definition = definition,
                        Step = step,
                        CancellationToken = cancellationToken
                    };

                    logger.LogDebug("Starting step {0} on workflow {1}", context.Step.Name, context.Definition.Id);

                    await context.Step.RunAsync(context);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Workflow {0} raised error on step {1} Message: {2}", definition.Id, step.Id, ex.Message);

                    result.Errors.Add(new ExecutionError
                    {
                        WorkflowId = definition.Id,
                        ErrorTime = DateTime.UtcNow,
                        Message = ex.Message
                    });
                }

                step = definition.GetNextStep(step);
            }

            return result;
        }

        public async Task<WorkflowExecutorResult> Execute(Flow flow, CancellationToken cancellationToken = default)
        {
            var workflow = flowConvertService.ConvertFlow(flow);

            var result = await Execute(workflow, cancellationToken);

            return result;
        }
    }
}