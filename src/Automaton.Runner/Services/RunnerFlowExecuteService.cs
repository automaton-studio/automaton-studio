using AutoMapper;
using Automaton.Core.Logs;
using Automaton.Core.Models;
using Automaton.Core.Services;
using Automaton.Runner.Models;
using Automaton.Runner.Services;
using MediatR;
using Serilog;
using Serilog.Context;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Automaton.Runner;

public class RunnerFlowExecuteService
{
    private readonly IMapper mapper;
    private readonly IMediator mediator;
    private readonly ILogger logger;
    private readonly HttpClient httpClient;
    private readonly ConfigurationService configurationService;
    private readonly FlowConvertService flowConvertService;
    private readonly FlowExecutionsService flowExecutionsService;

    public RunnerFlowExecuteService(FlowConvertService flowConvertService, FlowExecutionsService flowExecutionsService, 
        ConfigurationService configurationService, HttpClient httpClient, 
        IMediator mediator, IMapper mapper)
    {
        this.mapper = mapper;
        this.mediator = mediator;
        this.httpClient = httpClient;
        this.configurationService = configurationService;
        this.flowConvertService = flowConvertService;
        this.flowExecutionsService = flowExecutionsService;
        logger = Log.ForContext<CoreFlowExecuteService>();
    }

    public async Task<WorkflowExecution> Execute(Flow flow, CancellationToken cancellationToken = default)
    {
        var workflow = flowConvertService.ConvertFlow(flow);

        var workflowExecution = await Execute(workflow, cancellationToken);

        await SaveWorkflowExecution(workflowExecution);

        return workflowExecution;
    }

    private async Task<WorkflowExecution> Execute(Workflow workflow, CancellationToken cancellationToken = default)
    {
        using var workflowExecution = new WorkflowExecution(workflow.Id);

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
                workflowExecution.HasErrors();
            }

            step = step.GetNextStep();
        }

        logger.Information("End workflow: {0}", workflow.Name);

        return workflowExecution;
    }

    private async Task SaveWorkflowExecution(WorkflowExecution workflowExecution)
    {
        var flowExecution = mapper.Map<FlowExecution>(workflowExecution);
        await flowExecutionsService.Add(flowExecution);
    }
}
