using AutoMapper;
using Automaton.Studio.Models;

namespace Automaton.Studio.Services;

public class FlowScheduleService
{
    private HttpClient httpClient;
    private readonly ConfigurationService configService;
    private readonly IMapper mapper;
    private readonly ILogger logger;

    public FlowScheduleService
    (
        ConfigurationService configService,
        IMapper mapper,
        HttpClient httpClient
    )
    {
        this.logger = Log.ForContext<FlowScheduleService>(); ;
        this.configService = configService;
        this.httpClient = httpClient;
        this.mapper = mapper;
    }

    public async Task<IEnumerable<FlowSchedule>> GetFlowSchedules(Guid flowId)
    {
        try
        {
            var result = await httpClient.GetAsync($"{configService.FlowScheduleUrl}/{flowId}");

            result.EnsureSuccessStatusCode();

            var flowSchedules = await result.Content.ReadAsAsync<IEnumerable<FlowSchedule>>();

            return flowSchedules;
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Failed to load flow schedule list");

            throw;
        }
    }
}
