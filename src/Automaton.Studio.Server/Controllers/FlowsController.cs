using AutoMapper;
using Automaton.Core.Interfaces;
using Automaton.Studio.Server.Models;
using Automaton.Studio.Server.Services;
using Automaton.Studio.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Automaton.Studio.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlowsController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IFlowLoader definitionLoader;
        private readonly IWorkflowExecutor workflowExecutor;
        private readonly FlowsService flowsService;

        public FlowsController
        (
            FlowsService flowsService,
            IWorkflowExecutor workflowExecutor,
            IFlowLoader definitionLoader,
            IMapper mapper
        )
        {
            this.flowsService = flowsService;
            this.definitionLoader = definitionLoader;
            this.workflowExecutor = workflowExecutor;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<Flow>> Get() =>
            await flowsService.GetAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Flow>> Get(string id)
        {
            var flow = await flowsService.GetAsync(id);

            if (flow is null)
            {
                return NotFound();
            }

            return Ok(flow);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Flow newFlow)
        {
            await flowsService.CreateAsync(newFlow);

            return CreatedAtAction(nameof(Get), new { id = newFlow.Id }, newFlow);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, Flow updatedFlow)
        {
            var flow = await flowsService.GetAsync(id);

            if (flow is null)
            {
                return NotFound();
            }

            updatedFlow.Id = flow.Id;

            await flowsService.UpdateAsync(id, updatedFlow);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var flow = await flowsService.GetAsync(id);

            if (flow is null)
            {
                return NotFound();
            }

            await flowsService.RemoveAsync(flow.Id);

            return NoContent();
        }
    }
}