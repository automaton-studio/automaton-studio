using Automaton.Client.Auth.Http;
using Automaton.Runner.Storage;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Automaton.Runner.Services;

public class RunnerService
{
    private readonly ConfigService configService;
    private readonly HttpClient httpClient;
    private readonly ApplicationService applicationService;

    public RunnerService(AutomatonHttpClient httpClient, ConfigService configService, ApplicationService applicationStorage)
    {
        this.httpClient = httpClient.Client;
        this.configService = configService;
        this.applicationService = applicationStorage;
    }

    public async Task Register(string name, string serverUrl)
    {
        var runnerNameJson = JsonConvert.SerializeObject(new { Name = name });
        var runnerNameContent = new StringContent(runnerNameJson, Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync(serverUrl, runnerNameContent);

        response.EnsureSuccessStatusCode();

        applicationService.SetRunnerName(name);
    }
}
