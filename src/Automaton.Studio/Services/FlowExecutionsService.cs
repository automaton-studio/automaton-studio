using AutoMapper;
using Automaton.Studio.Models;

namespace Automaton.Studio.Services;

public class FlowExecutionsService
{
    private HttpClient httpClient;
    private readonly ConfigurationService configService;
    private readonly IMapper mapper;
    private readonly ILogger logger;

    public FlowExecutionsService
    (
        ConfigurationService configService,
        IMapper mapper,
        HttpClient httpClient
    )
    {
        this.logger = Log.ForContext<FlowExecutionsService>(); ;
        this.configService = configService;
        this.httpClient = httpClient;
        this.mapper = mapper;
    }

    public async Task<FlowExecutionResult> GetFlowExecutionResult(Guid flowId, int startIndex, int pageSize)
    {
        try
        {
            var result = await httpClient.GetAsync($"{configService.FlowExecutionUrl}/flow/{flowId}/{startIndex}/{pageSize}");

            result.EnsureSuccessStatusCode();

            var flowExecutionResult = await result.Content.ReadAsAsync<FlowExecutionResult>();

            return flowExecutionResult;
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Failed to load flow execution list");

            throw;
        }
    }

    public async Task<IEnumerable<LogModel>> GetLogsActivity(Guid flowId, Guid flowExecutionId)
    {
        try
        {
            var response = await httpClient.GetAsync($"{configService.FlowExecutionUrl}/logs/{flowExecutionId}");

            response.EnsureSuccessStatusCode();

            var logs = await response.Content.ReadAsAsync<IEnumerable<LogModel>>();

            return logs;

        }
        catch (Exception ex)
        {
            logger
                .ForContext("FlowId", flowId)
                .ForContext("FlowExecutionId", flowExecutionId)
                .Error(ex, "Failed to load flow execution logs");

            throw;
        }
    }

    public async Task<string> GetLogsActivityText(Guid flowId, Guid flowExecutionId)
    {
        try
        {
            var logs = await GetLogsActivity(flowId, flowExecutionId);

            var logsText = string.Join(Environment.NewLine, logs.Select(x => $"{x.Timestamp:s} {x.Level} {x.Message}"));

            return logsText;
        }
        catch (Exception ex)
        {
            logger
                .ForContext("FlowId", flowId)
                .ForContext("FlowExecutionId", flowExecutionId)
                .Error(ex, "Failed to load flow execution logs text");

            throw;
        }
    }

    public async Task Add(FlowExecution flowExecution)
    {
        var flowExecutionModel = new 
        {
            flowExecution.Id,
            flowExecution.FlowId,
            flowExecution.Started,
            flowExecution.Finished,
            Status = flowExecution.Status.ToString()
        };

        var response = await httpClient.PostAsJsonAsync(configService.FlowExecutionUrl, flowExecutionModel);
        response.EnsureSuccessStatusCode();
    }
}
