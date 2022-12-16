using AntDesign;
using Automaton.Studio.Logging;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Threading.Tasks;

namespace Automaton.Studio.Services;

public class ErrorService
{
    private readonly ILogger<ErrorService> logger;
    private readonly IMessageService messageService;

    public ErrorService(ILogger<ErrorService> logger, IMessageService messageService)
    {
        this.logger = logger;
        this.messageService = messageService;
    }

    public async Task ProcessError(Exception ex)
    {
        await messageService.Error("Something has gone wrong. Please contact support");

        var message = new LogMessage
        {
            LogLevel = LogLevel.Error.ToString(),
            ExceptionMessage = ex.Message,
            StackTrace = ex.StackTrace,
            Source = "Automaton",
            CreatedDate = DateTime.Now,
        };

        logger.Log(LogLevel.Error, 0, message, null, (logMessage, _) => JsonSerializer.Serialize(logMessage));
    }
}

