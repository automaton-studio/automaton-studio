using Automaton.Core.Services;
using Microsoft.Extensions.Logging;

namespace Automaton.Core.Logs;

public class WorkflowLogger
{
    protected readonly ILogger logger;

    public WorkflowLogger(ILoggerFactory loggerFactory)
    {
        logger = loggerFactory.CreateLogger<WorkflowExecuteService>();
    }
}
