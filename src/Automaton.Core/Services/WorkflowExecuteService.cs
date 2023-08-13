using Automaton.Core.Events;
using Automaton.Core.Logs;
using Automaton.Core.Models;
using MediatR;
using Serilog;
using Serilog.Context;

namespace Automaton.Core.Services;

public class WorkflowExecuteService
{
    private readonly IMediator mediator;
    private readonly ILogger logger;
    private readonly WorkflowConvertService flowConvertService;

    public WorkflowExecuteService(WorkflowConvertService flowConvertService, IMediator mediator)
    {
        this.mediator = mediator;
        this.flowConvertService = flowConvertService;
        logger = Log.ForContext<WorkflowExecuteService>();
    }

    private async Task<WorkflowExecutorResult> Execute(Workflow workflow, int executeDelay = 0, CancellationToken cancellationToken = default)
    {
        var result = new WorkflowExecutorResult();
        var definition = workflow.GetStartupDefinition();
        var step = definition.GetFirstStep();

        LogContext.PushProperty(LogProperties.Workflow, true);
        LogContext.PushProperty(LogProperties.WorkflowId, workflow.Id);
        LogContext.PushProperty(LogProperties.WorkflowName, workflow.Name);

        logger.Information("[Start workflow] {0}", workflow.Name);

        while (step != null)
        {
            var context = new StepExecutionContext
            {
                Workflow = workflow,
                Definition = definition,
                Step = step,
                CancellationToken = cancellationToken,
            };

            try
            {
                logger.Information("[Execute] {0}", step.Name);

                await mediator.Publish(new ExecuteStepNotification { StepId = step.Id }, cancellationToken);

                if (executeDelay > 0)
                {
                    await Task.Delay(executeDelay, cancellationToken);
                }

                await step.ExecuteAsync(context);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "[Error] Step {0} encountered an error. Message: {1}", step.Id, ex.Message);

                result.Errors.Add(new ExecutionError
                {
                    WorkflowId = definition.Id,
                    ErrorTime = DateTime.UtcNow,
                    Message = ex.Message
                });
            }

            step = step.GetNextStep();
        }

        logger.Information("[End workflow] {0}", workflow.Name);

        return result;
    }

    public async Task<WorkflowExecutorResult> Execute(Flow flow, int executeDelay = 0, CancellationToken cancellationToken = default)
    {
        var workflow = flowConvertService.ConvertFlow(flow);

        workflow.SetWorkflowVariable += async (sender, e) =>
        {
            await mediator.Publish(new SetVariableNotification { Variable = e.Variable }, cancellationToken);
        };

        var result = await Execute(workflow, executeDelay, cancellationToken);

        return result;
    }
}
