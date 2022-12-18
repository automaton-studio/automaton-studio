using Automaton.Studio.Server.Models;
using Automaton.Studio.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Serilog.Events;

namespace Automaton.Studio.Server.Controllers;

public class LogsController : BaseController
{
    private readonly LogsService logsService;

    public LogsController(LogsService logsService, ILogger<LogsController> logger)
    {
        this.logsService = logsService;
    }

    private readonly ILogger<LogsController> logger;

    public LogsController(ILogger<LogsController> logger)
    {
        this.logger = logger;
    }

    [HttpPost]
    public void Post([FromBody] LogEvent[] body)
    {
        var nbrOfEvents = body.Length;
        var apiKey = Request.Headers["X-Api-Key"].FirstOrDefault();

        logger.LogInformation(
            "Received batch of {count} log events from {sender}",
            nbrOfEvents,
            apiKey);

        foreach (var logEvent in body)
        {
            logger.LogInformation("Message: {message}", logEvent.RenderMessage());
        }
    }
}
