using Automaton.Runner.Http;
using Automaton.Runner.Models;
using Automaton.Runner.Storage;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Automaton.Runner.Services;

public class RunnerService
{
    private readonly ConfigurationService configService;
    private readonly HttpClient httpClient;
    private readonly ApplicationStorage applicationService;

    public RunnerService(RunnerHttpClient httpClient, ConfigurationService configService, ApplicationStorage applicationStorage)
    {
        this.httpClient = httpClient.Client;
        this.configService = configService;
        this.applicationService = applicationStorage;
    }

    public async Task SetupRunnerDetails(string runnerName)
    {
        var runnerNameJson = JsonConvert.SerializeObject(new RunnerDetails { Name = runnerName });
        var runnerNameContent = new StringContent(runnerNameJson, Encoding.UTF8, "application/json");
        var response = await httpClient.PostAsync($"{configService.RunnersUrl}", runnerNameContent);
        response.EnsureSuccessStatusCode();

        response = await httpClient.GetAsync($"{configService.RunnersUrl}/byname/{runnerName}");
        response.EnsureSuccessStatusCode();
        var runner = await response.Content.ReadAsAsync<RunnerDetails>();

        applicationService.SetRunnerId(runner.Id);
        applicationService.SetRunnerName(runner.Name);
    }

    public async Task UpdateRunnerDetails(string runnerName)
    {
        var runnerNameJson = JsonConvert.SerializeObject(new RunnerDetails { Name = runnerName });
        var runnerContent = new StringContent(runnerNameJson, Encoding.UTF8, "application/json");
        var response = await httpClient.PutAsync($"{configService.RunnersUrl}/{configService.RunnerId}", runnerContent);
        response.EnsureSuccessStatusCode();

        applicationService.SetRunnerName(runnerName);
    }
}
