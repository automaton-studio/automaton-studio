using AutoMapper;
using Automaton.Studio.Domain;
using Serilog;
using System.Net.Http;
using System.Threading.Tasks;

namespace Automaton.Studio.Services;

public class CustomStepsService
{
    private HttpClient httpClient;
    private readonly ConfigurationService configService;
    private readonly ILogger logger;

    public CustomStepsService
    (
        ConfigurationService configService,
        HttpClient httpClient
    )
    {
        logger = Log.ForContext<CustomStepsService>();
        this.configService = configService;
        this.httpClient = httpClient;
    }

    public async Task<ICollection<CustomStep>> List()
    {
        try
        {
            var result = await httpClient.GetAsync(configService.CustomStepsUrl);

            result.EnsureSuccessStatusCode();

            var steps = await result.Content.ReadAsAsync<ICollection<CustomStep>>();

            return steps;
        }
        catch (Exception ex)
        {
            logger.Error(ex, "An error happened when loading custom steps list.");
            throw;
        }   
    }

    public async Task<CustomStep> Load(Guid id)
    {
        var response = await httpClient.GetAsync($"{configService.CustomStepsUrl}/{id}");

        response.EnsureSuccessStatusCode();

        var customStep = await response.Content.ReadAsAsync<CustomStep>();

        return customStep;
    }

    public async Task<CustomStep> Create(string name, string displayName, string description)
    {
        var step = new CustomStep
        {
            Name = name,
            DisplayName = displayName,
            Description = description,
            Type = nameof(CustomStep),
            Category = "Custom Steps",
            Icon = "code"
        };

        var response = await httpClient.PostAsJsonAsync(configService.CustomStepsUrl, step);

        response.EnsureSuccessStatusCode();

        var newCustomStep = await response.Content.ReadAsAsync<CustomStep>();

        return newCustomStep;
    }

    public async Task Update(CustomStep customStep)
    {
        var response = await httpClient.PutAsJsonAsync($"{configService.CustomStepsUrl}/{customStep.Id}", customStep);

        response.EnsureSuccessStatusCode();
    }

    public async Task Delete(Guid id)
    {
        var response = await httpClient.DeleteAsync($"{configService.CustomStepsUrl}/{id}");

        response.EnsureSuccessStatusCode();
    }

    public async Task<bool> Exists(string name)
    {
        var result = await httpClient.GetAsync($"{configService.CustomStepsUrl}/exists/{name}");

        var exists = result.StatusCode != System.Net.HttpStatusCode.NotFound;

        return exists;
    }
}
