using Automaton.Studio.Resources;
using Automaton.Studio.Services;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
namespace Automaton.Studio.Logging;

public class ApiLogger : ILogger
{
    private readonly HttpClient httpClient;
    private readonly ConfigurationService configService;

    public ApiLogger(HttpClient httpClient, ConfigurationService configService)
    {
        this.httpClient = httpClient;
        this.configService = configService;
    }

    public IDisposable BeginScope<TState>(TState state)
    {
        return null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        var log = new LogMessage
        {
            LogLevel = logLevel.ToString(),
            EventName = eventId.Name,
            Message = exception?.Message ?? JsonSerializer.Serialize(state),
            StackTrace = exception?.StackTrace,
            Source = AppInfo.AutomatonStudio,
            CreatedDate = DateTime.Now
        };

        Task.Run(async () => await httpClient.PostAsJsonAsync(configService.LogsUrl, log));
    }
}