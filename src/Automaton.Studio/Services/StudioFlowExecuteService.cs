using Automaton.Core.Events;
using Automaton.Core.Logs;
using Automaton.Core.Models;
using Automaton.Core.Services;
using MediatR;
using Newtonsoft.Json;
using Serilog.Context;
using Serilog;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Automaton.Studio.Services;

public class StudioFlowExecuteService
{
    private readonly IMediator mediator;
    private readonly ILogger logger;
    private readonly HttpClient httpClient;
    private readonly ConfigurationService configurationService;
    private readonly FlowConvertService flowConvertService;

    public StudioFlowExecuteService(FlowConvertService flowConvertService, ConfigurationService configurationService, HttpClient httpClient, IMediator mediator) 
    {
        this.mediator = mediator;
        this.httpClient = httpClient;
        this.configurationService = configurationService;
        this.flowConvertService = flowConvertService;
        logger = Log.ForContext<CoreFlowExecuteService>();
    }

    public async Task<WorkflowExecution> Execute(Flow flow, int executeDelay = 0, CancellationToken cancellationToken = default)
    {
        var workflow = flowConvertService.ConvertFlow(flow);

        var result = await Execute(workflow, executeDelay, cancellationToken);

        return result;
    }

    private async Task<WorkflowExecution> Execute(Workflow workflow, int executeDelay = 0, CancellationToken cancellationToken = default)
    {
        workflow.SetWorkflowVariable += async (sender, e) =>
        {
            await mediator.Publish(new SetVariableNotification(e.Variable), cancellationToken);
        };

        var workflowExecution = new WorkflowExecution();
        workflowExecution.Start(workflow.Id);

        var definition = workflow.GetStartupDefinition();
        var step = definition.GetFirstStep();

        LogContext.PushProperty(LogContextProperties.WorkflowId, workflow.Id);
        LogContext.PushProperty(LogContextProperties.WorkflowExecutionId, workflowExecution.Id);
        LogContext.PushProperty(LogContextProperties.WorkflowName, workflow.Name);

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
                await DelayStepExecution(executeDelay, cancellationToken);

                await SendStepExecutionNotification(step, cancellationToken);

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

        await SendWorkflowExecutionResult(workflowExecution);

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

    private async Task SendWorkflowExecutionResult(WorkflowExecution workflowExecution)
    {
        var workflowExecutionJson = JsonConvert.SerializeObject(workflowExecution);
        var workflowExecutionContent = new StringContent(workflowExecutionJson, Encoding.UTF8, "application/json");
        var response = await httpClient.PostAsync($"{configurationService.FlowExecutionUrl}", workflowExecutionContent);
        response.EnsureSuccessStatusCode();
    }
}
