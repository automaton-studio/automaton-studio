using Automaton.Client.Auth.Http;
using Automaton.Runner.Models;
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
    private readonly ApplicationStorage applicationService;

    public RunnerService(AutomatonHttpClient httpClient, ConfigService configService, ApplicationStorage applicationStorage)
    {
        this.httpClient = httpClient.Client;
        this.configService = configService;
        this.applicationService = applicationStorage;
    }

    public async Task SetupRunnerDetails(string runnerName, string serverUrl)
    {
        // Need to use serverUrl because configuration is not updated,
        // so AutomatonHttpClient does not know what's the BaseUrl yet.

        var runnerNameJson = JsonConvert.SerializeObject(new RunnerDetails { Name = runnerName });
        var runnerNameContent = new StringContent(runnerNameJson, Encoding.UTF8, "application/json");
        var response = await httpClient.PostAsync($"{serverUrl}/{configService.RunnersUrl}", runnerNameContent);
        response.EnsureSuccessStatusCode();

        response = await httpClient.GetAsync($"{serverUrl}/{configService.RunnersUrl}/byname/{runnerName}");
        response.EnsureSuccessStatusCode();
        var runner = await response.Content.ReadAsAsync<RunnerDetails>();

        applicationService.SetServerUrl(serverUrl);
        applicationService.SetRunnerId(runner.Id);
        applicationService.SetRunnerName(runner.Name);
    }

    public async Task UpdateRunnerName(string runnerName)
    {
        var runnerNameJson = JsonConvert.SerializeObject(new RunnerDetails { Name = runnerName });
        var runnerContent = new StringContent(runnerNameJson, Encoding.UTF8, "application/json");
        var response = await httpClient.PutAsync($"{configService.RunnersUrl}/{configService.RunnerId}", runnerContent);
        response.EnsureSuccessStatusCode();

        applicationService.SetRunnerName(runnerName);
    }
}
