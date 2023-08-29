using Automaton.Core.Models;
using MediatR;
using Serilog;

namespace Automaton.Core.Services;

public class CoreFlowExecuteService
{
    private readonly IMediator mediator;
    private readonly ILogger logger;
    private readonly FlowConvertService flowConvertService;

    public CoreFlowExecuteService(FlowConvertService flowConvertService, IMediator mediator)
    {
        this.mediator = mediator;
        this.flowConvertService = flowConvertService;
        logger = Log.ForContext<CoreFlowExecuteService>();
    }

    public async Task<WorkflowExecution> Execute(Flow flow, CancellationToken cancellationToken = default)
    {
        var workflow = flowConvertService.ConvertFlow(flow);

        var result = await Execute(workflow, cancellationToken);

        return result;
    }

    public async Task<WorkflowExecution> Execute(Workflow workflow, CancellationToken cancellationToken = default)
    {
        using var workflowExecution = new WorkflowExecution(workflow.Id);

        var definition = workflow.GetStartupDefinition();
        var step = definition.GetFirstStep();

        logger.Information("Start workflow: {0}", workflow.Name);

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
                await step.ExecuteAsync(context);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Step: {0} encountered an error. Message: {1}", step.Id, ex.Message);
                workflowExecution.HasErrors();
            }

            step = step.GetNextStep();
        }

        logger.Information("End workflow: {0}", workflow.Name);

        return workflowExecution;
    }
}
