using Automaton.Studio.Server.Migrations;
using Automaton.Studio.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Core.Enrichers;
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
    public void Post(LogEvent[] logs)
    {
        foreach (var log in logs)
        {
            Enum.TryParse(log.Level, out LogEventLevel logLevel);

            using (LogContext.PushProperty("ApplicationName", "Automaton.Studio.Client"))
            {
                foreach (var property in log.Properties)
                {
                    LogContext.PushProperty(property.Key, property.Value);
                }

                var exception = HasException(log.Exception) ? GetException(log.Exception) : null;

                Log.Write(logLevel, exception, log.MessageTemplate);
            }
        }
    }

    private static bool HasException(string? exceptionText)
    {
        return !string.IsNullOrEmpty(exceptionText);
    }

    private static Exception GetException(string? exceptionText)
    {
        var exception = new Exception(CleanupExceptionText(exceptionText));

        return exception;
    }

    private static string CleanupExceptionText(string exceptionText)
    {
        var systemExceptionText = "System.Exception:";

        int index = exceptionText.IndexOf(systemExceptionText);

        string cleanExcptionText = (index < 0)
            ? exceptionText
            : exceptionText.Remove(index, systemExceptionText.Length);

        return cleanExcptionText.Trim();
    }
}
