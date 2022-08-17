using Automaton.Studio.Errors;
using Automaton.Studio.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
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
            var result = await httpClient.GetAsync(configService.FlowsUrl);

            result.EnsureSuccessStatusCode();

            var flows = await result.Content.ReadAsAsync<ICollection<FlowInfo>>();

            return flows;
        }
        catch (Exception ex)
        {
            logger.LogError(AppLogEvents.Error, ex, "Failed to load flows list");
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
