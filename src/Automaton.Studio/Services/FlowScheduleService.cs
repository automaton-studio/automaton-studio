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

    public async Task<IList<FlowScheduleModel>> List(Guid flowId)
    {
        try
        {
            var result = await httpClient.GetAsync($"{configService.FlowScheduleUrl}/{flowId}");

            result.EnsureSuccessStatusCode();

            var flowSchedules = await result.Content.ReadAsAsync<IList<FlowScheduleModel>>();

            return flowSchedules;
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Failed to load flow schedule list");

            throw;
        }
    }

    public async Task Create(FlowScheduleModel schedule)
    {
        var response = await httpClient.PostAsJsonAsync(configService.FlowScheduleUrl, schedule);
        response.EnsureSuccessStatusCode();
    }

    public async Task Update(FlowScheduleModel schedule)
    {
        var response = await httpClient.PutAsJsonAsync($"{configService.FlowScheduleUrl}/{schedule.Id}", schedule);
        response.EnsureSuccessStatusCode();
    }

    public async Task Delete(Guid id)
    {
        var response = await httpClient.DeleteAsync($"{configService.FlowScheduleUrl}/{id}");
        response.EnsureSuccessStatusCode();
    }
}
