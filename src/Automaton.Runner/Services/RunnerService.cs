using Automaton.Client.Auth.Http;
using Automaton.Runner.Storage;
using MediatR;
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

    public async Task Register(string name, string serverUrl)
    {
        RegisterClientSettings(name, serverUrl);
        await RegisterServerSettings(name, serverUrl);
    }

    private void RegisterClientSettings(string name, string serverUrl)
    {
        applicationService.SetServerUrl(serverUrl);
        applicationService.SetRunnerName(name);
    }

    private async Task RegisterServerSettings(string runnerName, string serverUrl)
    {
        var runnerNameJson = JsonConvert.SerializeObject(new { Name = runnerName });
        var runnerNameContent = new StringContent(runnerNameJson, Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync($"{serverUrl}/{configService.RegistrationUrl}", runnerNameContent);

        response.EnsureSuccessStatusCode();
    }
}
