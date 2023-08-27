using Automaton.Studio.Server.Models;
using Automaton.Studio.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace Automaton.Studio.Server.Controllers
{
    public class FlowExecutionsController : BaseController
    {
        private readonly FlowExecutionService flowExecutionService;

        public FlowExecutionsController(FlowExecutionService flowExecutionService)
        {
            this.flowExecutionService = flowExecutionService;
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

        [HttpPost]
        public ActionResult Post(FlowExecution runner, CancellationToken cancellationToken)
        {
            return Ok(flowExecutionService.Add(runner));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, FlowExecution flowExecution, CancellationToken cancellationToken)
        {
            await flowExecutionService.Update(id, flowExecution, cancellationToken);

            return NoContent();
        }
    }
}
