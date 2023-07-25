using Automaton.Client.Auth.Services;

namespace Automaton.Client.Auth.Http;

public class AutomatonHttpClient
{
    public HttpClient Client { get; }

    public AutomatonHttpClient(HttpClient httpClient, ClientAuthConfigurationService configService)
    {
        Client = httpClient;
        Client.BaseAddress = new Uri(configService.BaseUrl);
        Client.Timeout = new TimeSpan(0, 0, 30);
        Client.DefaultRequestHeaders.Clear();
    }
}
