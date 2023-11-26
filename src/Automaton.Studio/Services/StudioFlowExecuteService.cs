using AutoMapper;
using Automaton.Core.Logs;
using Automaton.Core.Models;
using Automaton.Studio.Domain;
using Automaton.Studio.Events;
using Automaton.Studio.Models;

namespace Automaton.Studio.Services;

public class StudioFlowExecuteService
{
    private readonly IMediator mediator;
    private readonly IMapper mapper;
    private readonly ILogger logger;
    private readonly ConfigurationService configurationService;
    private readonly StudioFlowConvertService flowConvertService;
    private readonly FlowExecutionsService flowExecutionsService;
    
    public StudioFlowExecuteService
    (
        StudioFlowConvertService flowConvertService, 
        FlowExecutionsService flowExecutionsService,
        ConfigurationService configurationService, 
        IMediator mediator, 
        IMapper mapper
    ) 
    {
        this.mediator = mediator;
        this.mapper = mapper;
        this.configurationService = configurationService;
        this.flowConvertService = flowConvertService;
        this.flowExecutionsService = flowExecutionsService;
        logger = Log.ForContext<StudioFlowExecuteService>();
    }

    public async Task<WorkflowExecution> Execute(StudioFlow flow, int executeDelay = 0, CancellationToken cancellationToken = default)
    {
        var workflow = flowConvertService.ConvertFlow(flow);

        workflow.SetWorkflowVariable += async (sender, e) =>
        {
            await mediator.Publish(new VariableUpdateNotification(e.Variable), cancellationToken);
        };

        var workflowExecution = await ExecuteWorkflow(workflow, executeDelay, cancellationToken);

        await SaveWorkflowExecution(workflowExecution);

        return workflowExecution;
    }

    private async Task<WorkflowExecution> ExecuteWorkflow(Workflow workflow, int executeDelay = 0, CancellationToken cancellationToken = default)
    {
        using var workflowExecution = new WorkflowExecution(workflow.Id, configurationService.ApplicationName);
        var definition = workflow.GetStartupDefinition();
        var step = definition.GetFirstStep();

        LogContext.PushProperty(LogContextProperties.WorkflowId, workflow.Id);
        LogContext.PushProperty(LogContextProperties.WorkflowExecutionId, workflowExecution.Id);
        LogContext.PushProperty(LogContextProperties.WorkflowName, workflow.Name);

        logger.Information("Start workflow: {0}", workflow.Name);

        while (step != null && !cancellationToken.IsCancellationRequested)
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
                await DelayStepExecution(executeDelay, cancellationToken);

                await SendStepExecutionNotification(step, cancellationToken);

                await step.ExecuteAsync(context);
            }
            catch (OperationCanceledException)
            {
                logger.Information("Workflow execution was canceled");
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Step {0} with Id {1} encountered an error. Message: {2}", step.Name, step.Id, ex.Message);
                workflowExecution.HasErrors();
            }

            step = step.GetNextStep();
        }

        logger.Information("End workflow: {0}", workflow.Name);

        return workflowExecution;
    }

    private static async Task DelayStepExecution(int executeDelay, CancellationToken cancellationToken)
    {
        await Task.Delay(executeDelay, cancellationToken);
    }

    private async Task SendStepExecutionNotification(WorkflowStep step, CancellationToken cancellationToken)
    {
        await mediator.Publish(new ExecuteStepNotification(step.Id), cancellationToken);
    }

    private async Task SaveWorkflowExecution(WorkflowExecution workflowExecution)
    {
        var flowExecution = mapper.Map<FlowExecution>(workflowExecution);
        await flowExecutionsService.Add(flowExecution);
    }
}
