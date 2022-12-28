using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using System.Diagnostics;

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
                case "Information":
                    logger.LogInformation(log.MessageTemplate, log.Properties.Values);
                    break;
                case "Debug":
                    logger.LogDebug(log.MessageTemplate, log.Properties.Values);
                    break;
                case "Warning":
                    logger.LogWarning(log.MessageTemplate, log.Properties.Values);
                    break;
                case "Error":
                    logger.LogError(new Exception(log.MessageTemplate), log.RenderedMessage, log.Properties.Values);
                    break;
                case "Critical":
                    logger.LogCritical(new Exception(log.MessageTemplate), log.RenderedMessage, log.Properties.Values);
                    break;  
                default:
                    logger.LogInformation(log.MessageTemplate, log.Properties.Values);
                    break;
            }
        }
    }
}
