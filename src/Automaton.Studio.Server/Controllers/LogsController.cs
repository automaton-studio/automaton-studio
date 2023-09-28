using Automaton.Studio.Server.Entities;
using Automaton.Studio.Server.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Automaton.Studio.Server.Controllers;

public class LogsController : BaseController
{
    [HttpGet("flow/{flowId}/{startIndex}/{pageSize}")]
    public async Task<ActionResult<IEnumerable<Log>>> GetForFlow(Guid flowId, int startIndex, int pageSize, CancellationToken cancellationToken)
    {
        var logsQuery = new FlowLogsQuery
        {
            FlowId = flowId,
            StartIndex = startIndex,
            PageSize = pageSize
        };

        var result = await Mediator.Send(logsQuery, cancellationToken);

        return Ok(result);
    }
}