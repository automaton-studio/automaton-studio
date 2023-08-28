using Automaton.Core.Events;
using Automaton.Core.Models;
using MediatR;
using Serilog;

namespace Automaton.Core.Services;

public class FlowExecuteService
{
    private readonly IMediator mediator;
    private readonly ILogger logger;
    private readonly CoreFlowConvertService flowConvertService;

    public FlowExecuteService(CoreFlowConvertService flowConvertService, IMediator mediator)
    {
        this.mediator = mediator;
        this.flowConvertService = flowConvertService;
        logger = Log.ForContext<FlowExecuteService>();
    }

    public async Task<WorkflowExecution> Execute(Flow flow, CancellationToken cancellationToken = default)
    {
        var workflow = flowConvertService.ConvertFlow(flow);

        workflow.SetWorkflowVariable += async (sender, e) =>
        {
            await mediator.Publish(new SetVariableNotification(e.Variable), cancellationToken);
        };

        var result = await Execute(workflow, cancellationToken);

        return result;
    }

    public async Task<WorkflowExecution> Execute(Workflow workflow, CancellationToken cancellationToken = default)
    {
        var workflowExecution = new WorkflowExecution();
        workflowExecution.Start(workflow.Id);

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
                workflowExecution.Error();
            }

            step = step.GetNextStep();
        }

        logger.Information("End workflow: {0}", workflow.Name);

        workflowExecution.Finish();

        return workflowExecution;
    }
}
