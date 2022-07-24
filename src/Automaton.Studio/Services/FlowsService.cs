using Automaton.Studio.Errors;
using Automaton.Studio.Pages.Flows;
using Microsoft.Extensions.Logging;
using System;
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

    public async Task<ICollection<FlowModel>> List()
    {
        ICollection<FlowModel> flows = new List<FlowModel>();

        try
        {
            var result = await httpClient.GetAsync(configService.FlowsUrl);

            result.EnsureSuccessStatusCode();

            flows = await result.Content.ReadAsAsync<ICollection<FlowModel>>();
        }
        catch (Exception ex)
        {
            logger.LogError(AppLogEvents.Error, ex, "Failed to load flows list");
        }

        return flows;
    }

    public async Task<bool> Exists(string name)
    {
        var result = await httpClient.GetAsync($"{configService.FlowsUrl}/exists/{name}");

        var exists = result.StatusCode != System.Net.HttpStatusCode.NotFound;

        return exists;
    }
}
