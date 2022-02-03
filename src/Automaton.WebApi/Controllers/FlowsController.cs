using Automaton.WebApi.Models;
using Automaton.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace Conductor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlowsController : ControllerBase
    {
        private readonly FlowsService flowsService;

        public FlowsController(FlowsService service)
        {
            flowsService = service;
        }

        [HttpGet]
        public async Task<List<Flow>> Get() =>
            await flowsService.GetAsync();

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Flow>> Get(string id)
        {
            var flow = await flowsService.GetAsync(id);

            if (flow is null)
            {
                return NotFound();
            }

            return flow;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Flow newFlow)
        {
            await flowsService.CreateAsync(newFlow);

            return CreatedAtAction(nameof(Get), new { id = newFlow.Id }, newFlow);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Flow updatedFlow)
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

        [HttpDelete("{id:length(24)}")]
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