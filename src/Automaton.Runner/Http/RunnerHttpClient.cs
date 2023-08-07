using Automaton.Runner.Services;
using System;
using System.Net.Http;

namespace Automaton.Runner.Http;

public class RunnerHttpClient
{
    public HttpClient Client { get; }

    public RunnerHttpClient(HttpClient httpClient, ConfigService configService)
    {
        Client = httpClient;
        Client.BaseAddress = new Uri(configService.BaseUrl);
        Client.Timeout = new TimeSpan(0, 0, 30);
        Client.DefaultRequestHeaders.Clear();
    }
}
