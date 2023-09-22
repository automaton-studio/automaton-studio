using Automaton.Studio.Server.Models;
using Automaton.Studio.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace Automaton.Studio.Server.Controllers
{
    public class FlowExecutionsController : BaseController
    {
        private readonly FlowExecutionService flowExecutionService;
        private readonly LogsService logsService;

        public FlowExecutionsController(FlowExecutionService flowExecutionService, LogsService logsService)
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

        [HttpGet("flow/{flowId}")]
        public ActionResult<IEnumerable<FlowExecution>> GetForFlow(Guid flowId)
        {
            return Ok(flowExecutionService.GetForFlow(flowId));
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
