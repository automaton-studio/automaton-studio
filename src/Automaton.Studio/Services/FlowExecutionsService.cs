using AutoMapper;
using Automaton.Core.Models;
using Automaton.Studio.Domain;
using Automaton.Studio.Models;
using Serilog;
using System.Net.Http;
using System.Threading.Tasks;

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

    public async Task<ICollection<FlowExecution>> List()
    {
        ICollection<FlowExecution> flowExecutions = new List<FlowExecution>();

        try
        {
            var result = await httpClient.GetAsync(configService.FlowExecutionUrl);

            result.EnsureSuccessStatusCode();

            flowExecutions = await result.Content.ReadAsAsync<ICollection<FlowExecution>>();
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Failed to load flow execution list");
        }

        return flowExecutions;
    }

    public async Task Add(FlowExecution flowExecution)
    {
        var response = await httpClient.PostAsJsonAsync(configService.FlowExecutionUrl, flowExecution);
        response.EnsureSuccessStatusCode();
    }
}
