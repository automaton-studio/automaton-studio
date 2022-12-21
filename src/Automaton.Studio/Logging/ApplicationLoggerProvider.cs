using System.Net.Http;
using Automaton.Studio.Services;
using Microsoft.Extensions.Logging;

namespace Automaton.Studio.Logging;

public class ApplicationLoggerProvider : ILoggerProvider
{
    private readonly HttpClient httpClient;
    private readonly ConfigurationService configService;

    public ApplicationLoggerProvider(HttpClient httpClient, ConfigurationService configService)
    {
        this.httpClient = httpClient;
        this.configService = configService;
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new ApiLogger(httpClient, configService);
    }

    public void Dispose()
    {
    }
}
