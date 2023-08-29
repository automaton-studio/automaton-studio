using Automaton.Runner.Http;
using Automaton.Runner.Models;
using Automaton.Runner.Storage;
using Newtonsoft.Json;
using System;
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

    public async Task SetupRunner(string runnerName)
    {
        var runnerDetails = new RunnerDetails 
        { 
            Id = Guid.NewGuid().ToString(),
            Name = runnerName 
        };

        var response = await httpClient.PostAsJsonAsync(configService.RunnersUrl, runnerDetails);
        response.EnsureSuccessStatusCode();

        applicationService.SetRunnerId(runnerDetails.Id);
        applicationService.SetRunnerName(runnerDetails.Name);
    }

    public async Task UpdateRunner(string runnerName)
    {
        var runnerDetails = new RunnerDetails
        {
            Id = configService.RunnerId,
            Name = runnerName
        };

        var response = await httpClient.PutAsJsonAsync($"{configService.RunnersUrl}/{configService.RunnerId}", runnerDetails);
        response.EnsureSuccessStatusCode();

        applicationService.SetRunnerName(runnerName);
    }
}
