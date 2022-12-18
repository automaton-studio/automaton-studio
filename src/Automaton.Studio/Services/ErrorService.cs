using AntDesign;
using Serilog;
using System.Threading.Tasks;

namespace Automaton.Studio.Services;

public class ErrorService
{
    private readonly ILogger logger;
    private readonly IMessageService messageService;

    public ErrorService(ILogger logger, IMessageService messageService)
    {
        this.logger = logger;
        this.messageService = messageService;
    }

    public async Task ProcessError(Exception ex)
    {
        await messageService.Error("Something has gone wrong. Please contact support");

        Log.Debug("Hello, browser!");

        logger.Information("Hello, browser!");
    }
}

