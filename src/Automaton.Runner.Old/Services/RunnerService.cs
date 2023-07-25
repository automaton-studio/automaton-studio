using Automaton.Client.Auth.Http;
using Automaton.Runner.Core.Services;
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
    private readonly ApplicationStorage applicationStorage;

    public RunnerService(AutomatonHttpClient httpClient, ConfigService configService, ApplicationStorage applicationStorage)
    {
        this.httpClient = httpClient.Client;
        this.configService = configService;
        this.applicationStorage = applicationStorage;
    }

    public async Task Register(string runnerName)
    {
        var runnerNameJson = JsonConvert.SerializeObject(new { Name = runnerName });
        var runnerNameContent = new StringContent(runnerNameJson, Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync(configService.ApiConfig.RegistrationUrl, runnerNameContent);

        response.EnsureSuccessStatusCode();

        applicationStorage.SetRunnerName(runnerName);
    }
}
