﻿using AutoMapper;
using Automaton.Core.Logs;
using Automaton.Core.Scripting;
using Automaton.Studio.Domain;

namespace Automaton.Studio.Services;

public class CustomStepExecuteService : IDisposable
{
    private const string ScriptType = @"text/x-python";

    private readonly IMediator mediator;
    private readonly IMapper mapper;
    private readonly ILogger logger;
    private readonly IServiceProvider serviceProvider;
    private readonly ConfigurationService configurationService;
    private readonly StudioFlowConvertService flowConvertService;
    private readonly FlowExecutionsService flowExecutionsService;
    private readonly ScriptEngineHost scriptHost;

    public event EventHandler<string> ScriptTextWritten;

    public CustomStepExecuteService
    (
        StudioFlowConvertService flowConvertService, 
        FlowExecutionsService flowExecutionsService,
        ConfigurationService configurationService,
        ScriptEngineHost scriptHost,
        IServiceProvider serviceProvider,
        IMediator mediator, 
        IMapper mapper
    ) 
    {
        this.mediator = mediator;
        this.mapper = mapper;
        this.configurationService = configurationService;
        this.flowConvertService = flowConvertService;
        this.flowExecutionsService = flowExecutionsService;
        this.serviceProvider = serviceProvider;
        this.scriptHost = scriptHost;
        this.scriptHost.ScriptTextWritten += OnScriptTextWritten;

        logger = Log.ForContext<StudioFlowExecuteService>();
    }

    public CustomStepExecution Execute(CustomStep step, CancellationToken cancellationToken = default)
    {
        using var customStepExecution = new CustomStepExecution();

        LogContext.PushProperty(LogContextProperties.StepId, step.Id);
        LogContext.PushProperty(LogContextProperties.StepName, step.Name);

        logger.Information("Start step: {0}", step.Name);

        try
        {
            ExecuteAsync(step);
        }
        catch (Exception ex)
        {
            customStepExecution.HasErrors();

            logger.Error(ex, "Step: {0} encountered an error. Message: {1}", step.Name, ex.Message);

            throw;
        }

        logger.Information("End step: {0}", step.Name);

        return customStepExecution;
    }

    protected void ExecuteAsync(CustomStep step)
    {
        var resource = new ScriptResource()
        {
            ContentType = ScriptType,
            Content = step.Code
        };

        var inputVariablesDictionary = step.InputVariables.ToDictionary(x => x.Name, x => x.Value);

        var scriptVariables = scriptHost.Execute(resource, inputVariablesDictionary);

        foreach (var variable in step.OutputVariables)
        {
            if (scriptVariables.ContainsKey(variable.Id))
            {
                variable.Value = scriptVariables[variable.Id].ToString();
            }
        }
    }

    private void OnScriptTextWritten(object sender, string e)
    {
        ScriptTextWritten?.Invoke(this, e);
    }

    public void Dispose()
    {
        scriptHost.ScriptTextWritten -= OnScriptTextWritten;
    }
}
