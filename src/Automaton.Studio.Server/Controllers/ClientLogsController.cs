using Automaton.Core.Logs;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Context;
using Serilog.Events;

namespace Automaton.Studio.Server.Controllers;

public class ClientLogsController : BaseController
{
    private const string SystemException = "System.Exception:";

    private readonly Serilog.ILogger logger;

    public ClientLogsController()
    {
        logger = Log.ForContext<ClientLogsController>();
    }

    [HttpPost]
    public IActionResult Post(SerilogHttpLogEvent[] logs)
    {
        foreach (var log in logs)
        {
            Enum.TryParse(log.Level, out LogEventLevel logLevel);

            foreach (var property in log.Properties)
            {
                LogContext.PushProperty(property.Key, property.Value);
            }

            var exception = HasException(log.Exception) ? GetException(log.Exception) : null;

            logger.Write(logLevel, exception, log.MessageTemplate);
        }

        return NoContent();
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
        int index = exceptionText.IndexOf(SystemException);

        string cleanExcptionText = (index < 0)
            ? exceptionText
            : exceptionText.Remove(index, SystemException.Length);

        return cleanExcptionText.Trim();
    }
}
