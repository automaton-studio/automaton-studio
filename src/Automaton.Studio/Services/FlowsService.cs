using Automaton.Studio.Models;
using Serilog;
using System.Net.Http;
using System.Threading.Tasks;

namespace Automaton.Studio.Services;

public class FlowsService
{
    private HttpClient httpClient;
    private readonly ConfigurationService configService;
    private readonly Serilog.ILogger logger;

    public FlowsService
    (
        ConfigurationService configService,
        HttpClient httpClient
    )
    {
        logger = Log.ForContext<FlowsService>();
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
            logger.Error("An error happened when loading flows list. {Message} and {StackTrace}", ex.Message, ex.StackTrace);
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
