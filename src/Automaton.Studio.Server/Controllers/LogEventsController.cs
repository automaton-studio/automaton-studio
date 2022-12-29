using Automaton.Studio.Errors;
using Automaton.Studio.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace Automaton.Studio.Server.Controllers;

public class LogEventsController : BaseController
{
    private readonly Guid userId;
    private readonly ILogger<LogEventsController> logger;

    public LogEventsController(ILogger<LogEventsController> logger, UserContextService userContextService)
    {
        userId = userContextService.GetUserId(); ;
        this.logger = logger; 
    }

    [HttpPost]
    public void Post([FromBody] LogEvent[] logs)
    {
        logger.LogInformation("Received batch of {Count} log events from {User}", logs.Length, userId);

        foreach (var log in logs)
        {
            Enum.TryParse(log.Level, out LogLevel logLevel);

            log.Properties.Add("User", userId);

            logger.Log(logLevel, new EventId(LogEventId.Client, nameof(LogEventId.Client)), log.MessageTemplate, log.Properties);
        }
    }
}
