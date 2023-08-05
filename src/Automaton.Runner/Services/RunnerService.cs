using Automaton.Client.Auth.Http;
using Automaton.Runner.Models;
using Automaton.Runner.Storage;
using MediatR;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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
        applicationService.SetServerUrl(serverUrl);
        applicationService.SetRunnerName(runnerName);

        var runnerNameJson = JsonConvert.SerializeObject(new { Name = runnerName });
        var runnerNameContent = new StringContent(runnerNameJson, Encoding.UTF8, "application/json");
        var response = await httpClient.PostAsync($"{serverUrl}/{configService.RegistrationUrl}", runnerNameContent);
        response.EnsureSuccessStatusCode();

        response = await httpClient.GetAsync($"{serverUrl}/{configService.RunnerByNameUrl}/{runnerName}");
        response.EnsureSuccessStatusCode();
        var runner = await response.Content.ReadAsAsync<RunnerDetails>();

        applicationService.SetRunnerId(runner.Id);
    }

    public async Task UpdateRunnerName(string runnerName)
    {
        var runnerNameJson = JsonConvert.SerializeObject(new { Name = runnerName });
        var runnerNameContent = new StringContent(runnerNameJson, Encoding.UTF8, "application/json");

        var response = await httpClient.PutAsync($"{configService.BaseUrl}/{configService.RegistrationUrl}/{configService.RunnerId}", runnerNameContent);

        response.EnsureSuccessStatusCode();
    }
}
