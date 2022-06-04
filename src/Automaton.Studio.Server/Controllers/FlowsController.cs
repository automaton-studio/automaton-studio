using AutoMapper;
using Automaton.Core.Models;
using Automaton.Core.Services;
using Automaton.Studio.Server.Models;
using Automaton.Studio.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace Automaton.Studio.Server.Controllers
{
    public class FlowsController : BaseController
    {
        private readonly IMapper mapper;
        private readonly FlowLoaderService flowLoaderService;
        private readonly WorkflowExecuteService workflowExecuteService;
        private readonly FlowsService flowsService;

        public FlowsController
        (
            FlowsService flowsService,
            WorkflowExecuteService workflowExecuteService,
            FlowLoaderService definitionLoader,
            IMapper mapper
        )
        {
            this.flowsService = flowsService;
            this.flowLoaderService = definitionLoader;
            this.workflowExecuteService = workflowExecuteService;
            this.mapper = mapper;
        }

        [HttpGet]
        public IEnumerable<Entities.Flow> Get()
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
    }
}