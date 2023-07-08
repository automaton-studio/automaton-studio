using Automaton.Core.Logs;
using Automaton.Core.Models;
using Serilog;
using Serilog.Context;

namespace Automaton.Core.Services;

public class WorkflowExecuteService
{
    private readonly ILogger logger;
    private readonly WorkflowConvertService flowConvertService;

    public WorkflowExecuteService(WorkflowConvertService flowConvertService)
    {
        this.flowConvertService = flowConvertService;
        logger = Log.ForContext<WorkflowExecuteService>();
    }

    public async Task<WorkflowExecutorResult> Execute(Workflow workflow, CancellationToken cancellationToken = default)
    {
        var result = new WorkflowExecutorResult();
        var definition = workflow.GetStartupDefinition();
        var step = definition.GetFirstStep();

        using (LogContext.PushProperty("WorkflowId", workflow.Id))
        {
            using (LogContext.PushProperty("WorkflowName", workflow.Name))
            {
                logger.Information("Execute workflow {0}", workflow.Name);

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
                        logger.Information("Execute step {0}", step.Name);

                        await step.ExecuteAsync(context);
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex, "Step {0} encountered an error. Message: {1}", step.Id, ex.Message);

                        result.Errors.Add(new ExecutionError
                        {
                            WorkflowId = definition.Id,
                            ErrorTime = DateTime.UtcNow,
                            Message = ex.Message
                        });
                    }

                    step = step.GetNextStep();
                }
            }
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
