using Microsoft.AspNetCore.Mvc;
using Serilog.Events;

namespace Automaton.Studio.Server.Controllers;

public class LogEventsController : BaseController
{
    private readonly ILogger<LogEventsController> logger;

    public LogEventsController(ILogger<LogEventsController> logger)
    {
        this.logger = logger; 
    }

    [HttpPost]
    public void Post([FromBody] LogEvent[] logs)
    {
        var numberOfLogs = logs.Length;
        var apiKey = Request.Headers["X-Api-Key"].FirstOrDefault();

        logger.LogInformation("Received batch of {count} log events from {sender}", numberOfLogs, apiKey);

        foreach (var log in logs)
        {
            switch (log.Level)
            {
                //case LogEventLevel.Information:
                //    logger.LogInformation(log.MessageTemplate);
                //    break;
                //case LogEventLevel.Debug:
                //    logger.LogDebug(log.MessageTemplate);
                //    break;
                //case LogEventLevel.Warning:
                //    logger.LogWarning(log.MessageTemplate);
                //    break;
                //case LogEventLevel.Error:
                //    logger.LogError(new Exception(log.MessageTemplate), log.MessageTemplate);
                //    break;
                //case LogEventLevel.Fatal:
                //    logger.LogCritical(new Exception(log.MessageTemplate), log.MessageTemplate);
                //    break;
            }
        }
    }
}
