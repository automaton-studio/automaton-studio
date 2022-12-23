using AntDesign;
using Microsoft.Extensions.Logging;
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
        await messageService.Error(Resources.Errors.UnexpectedError);

        logger.LogError(ex, ex.Message);
    }

    public async Task ProcessError(Exception ex, string message)
    {
        await messageService.Error(message);

        logger.LogError(ex, ex.Message);
    }
}

