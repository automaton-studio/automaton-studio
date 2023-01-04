using Automaton.Studio.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Context;
using Serilog.Events;

namespace Automaton.Studio.Server.Controllers;

public class LogEventsController : BaseController
{
    private readonly Guid userId;
    private readonly Serilog.ILogger logger;

    public LogEventsController(UserContextService userContextService)
    {
        userId = userContextService.GetUserId(); ;
        logger = Log.ForContext<LogEventsController>();
    }

    [HttpPost]
    public void Post([FromBody] LogEvent[] logs)
    {
        foreach (var log in logs)
        {
            Enum.TryParse(log.Level, out LogEventLevel logLevel);

            using (LogContext.PushProperty("ApplicationName", "Automaton.Studio.Client"))
            {
                Log.Write(logLevel, log.MessageTemplate, log.Properties);
            }
        }
    }
}
