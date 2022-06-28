using Automaton.Core.Models;
using Automaton.Core.Services;
using Automaton.Studio.Server.Core.Commands;
using Automaton.Studio.Server.Models;
using Automaton.Studio.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace Automaton.Studio.Server.Controllers
{
    public class FlowsController : BaseController
    {
        private readonly FlowLoaderService flowLoaderService;
        private readonly FlowsService flowsService;

        public FlowsController
        (
            FlowsService flowsService,
            FlowLoaderService flowLoaderService
        )
        {
            this.flowsService = flowsService;
            this.flowLoaderService = flowLoaderService;
        }

        [HttpGet]
        public IEnumerable<FlowInfo> Get()
        {
            return flowsService.List();
        }

        [HttpGet("{id}")]
        public ActionResult<Flow> Get(Guid id)
        {
            var flow = flowsService.Get(id);

            if (flow is null)
            {
                return NotFound();
            }

            return Ok(flow);
        }

        [HttpPost]
        public IActionResult Post(Flow flow)
        {
            var flowId = flowsService.Create(flow);

            var newFlow = flowsService.Get(flowId);

            return CreatedAtAction(nameof(Get), new { id = newFlow.Id }, newFlow);
        }

        [HttpPut("{id}")]
        public IActionResult Put(Guid id, Flow flow)
        {
            flowsService.Update(id, flow);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            flowsService.Remove(id);

            return NoContent();
        }

        [HttpPost("run")]
        public async Task<ActionResult> Post(ExecuteFlowCommand command, CancellationToken cancellationToken)
        {
            return Ok(await Mediator.Send(command, cancellationToken));
        }
    }
}