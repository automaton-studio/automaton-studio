using Automaton.Core.Logs;
using Automaton.Core.Models;
using Automaton.Core.Services;
using Automaton.Runner.Services;
using MediatR;
using Newtonsoft.Json;
using Serilog;
using Serilog.Context;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Automaton.Runner;

public class RunnerFlowExecuteService
{
    private readonly IMediator mediator;
    private readonly ILogger logger;
    private readonly HttpClient httpClient;
    private readonly ConfigurationService configurationService;
    private readonly CoreFlowConvertService flowConvertService;

    public RunnerFlowExecuteService(CoreFlowConvertService flowConvertService, ConfigurationService configurationService, HttpClient httpClient, IMediator mediator)
    {
        this.mediator = mediator;
        this.httpClient = httpClient;
        this.configurationService = configurationService;
        this.flowConvertService = flowConvertService;
        logger = Log.ForContext<FlowExecuteService>();
    }

    public async Task<WorkflowExecution> Execute(Flow flow, CancellationToken cancellationToken = default)
    {
        var workflow = flowConvertService.ConvertFlow(flow);

        var result = await Execute(workflow, cancellationToken);

        await SendWorkflowExecutionResult(result);

        return result;
    }

    private async Task<WorkflowExecution> Execute(Workflow workflow, CancellationToken cancellationToken = default)
    {
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

    private async Task SendWorkflowExecutionResult(WorkflowExecution workflowExecution)
    {
        var workflowExecutionJson = JsonConvert.SerializeObject(workflowExecution);
        var workflowExecutionContent = new StringContent(workflowExecutionJson, Encoding.UTF8, "application/json");
        var response = await httpClient.PostAsync($"{configurationService.FlowExecutionUrl}", workflowExecutionContent);
        response.EnsureSuccessStatusCode();
    }
}
