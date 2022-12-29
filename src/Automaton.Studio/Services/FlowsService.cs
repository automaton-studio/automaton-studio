using Automaton.Studio.Models;
using Microsoft.Extensions.Logging;
using Serilog.Core;
using System.Net.Http;
using System.Threading.Tasks;

namespace Automaton.Studio.Services;

public class FlowsService
{
    private HttpClient httpClient;
    private readonly ConfigurationService configService;
    private readonly ILogger<FlowsService> logger;

    public FlowsService
    (
        ConfigurationService configService,
        HttpClient httpClient,
        ILogger<FlowsService> logger
    )
    {
        this.logger = logger;
        this.configService = configService;
        this.httpClient = httpClient;
    }

    public async Task<ICollection<FlowInfo>> List()
    {
        try
        {
            logger.LogInformation("Loading flows list for user {UserName}", "Razvan");

            var result = await httpClient.GetAsync(configService.FlowsUrl);

            result.EnsureSuccessStatusCode();

            var flows = await result.Content.ReadAsAsync<ICollection<FlowInfo>>();

            return flows;
        }
        catch (Exception ex)
        {
            logger.LogError("An error happened when loading flows lists. {Error}", new { ex.Message, ex.StackTrace});
            throw;
        }   
    }

    public async Task<bool> Exists(string name)
    {
        var result = await httpClient.GetAsync($"{configService.FlowsUrl}/exists/{name}");

        var exists = result.StatusCode != System.Net.HttpStatusCode.NotFound;

        return exists;
    }
}
