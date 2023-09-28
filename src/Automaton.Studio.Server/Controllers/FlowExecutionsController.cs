using Automaton.Studio.Server.Models;
using Automaton.Studio.Server.Queries;
using Automaton.Studio.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace Automaton.Studio.Server.Controllers
{
    public class FlowExecutionsController : BaseController
    {
        private readonly FlowExecutionService flowExecutionService;
        private readonly FlowLogsService logsService;

        public FlowExecutionsController(FlowExecutionService flowExecutionService, FlowLogsService logsService)
        {
            this.flowExecutionService = flowExecutionService;
            this.logsService = logsService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<FlowExecution>> Get()
        {
            return Ok(flowExecutionService.List());
        }

        [HttpGet("{id}")]
        public ActionResult<IEnumerable<FlowExecution>> Get(Guid id)
        {
            return Ok(flowExecutionService.Get(id));
        }

        [HttpGet("flow/{flowId}/{startIndex}/{pageSize}")]
        public async Task<ActionResult<IEnumerable<FlowExecution>>> GetForFlow(Guid flowId, int startIndex, int pageSize, CancellationToken cancellationToken)
        {
            var flowExecutionQuery = new FlowExecutionQuery
            {
                FlowId = flowId,
                StartIndex = startIndex,
                PageSize = pageSize
            };

            var result = await Mediator.Send(flowExecutionQuery, cancellationToken);

            return Ok(result);
        }

        [HttpGet("logs/{flowExecutionId}")]
        public ActionResult<IEnumerable<Entities.Log>> GetLogs(Guid flowExecutionId)
        {
            return Ok(logsService.GetFlowExecutionLogs(flowExecutionId));
        }

        [HttpPost]
        public ActionResult Post(FlowExecution flowExecution, CancellationToken cancellationToken)
        {
            return Ok(flowExecutionService.Add(flowExecution));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, FlowExecution flowExecution, CancellationToken cancellationToken)
        {
            await flowExecutionService.Update(id, flowExecution, cancellationToken);

            return NoContent();
        }
    }
}
