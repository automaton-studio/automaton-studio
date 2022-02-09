using AutoMapper;
using Automaton.Core.Models;
using Automaton.WebApi.Interfaces;
using Automaton.WebApi.Models;
using Automaton.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace Conductor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlowsController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IDefinitionLoader definitionLoader;
        private readonly FlowsService flowsService;
        
        public FlowsController(FlowsService flowsService, IDefinitionLoader definitionLoader, IMapper mapper)
        {
            this.flowsService = flowsService;
            this.definitionLoader = definitionLoader;
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

        [HttpGet]
        [Route("run/{id}")]
        public async Task<ActionResult<Flow>> Run(string id)
        {
            var flow = await flowsService.GetAsync(id);

            if (flow is null)
            {
                return NotFound();
            }

            var definition = flow.GetStartupDefinition();

            var workflowDefinition = definitionLoader.LoadDefinition(definition);

            return Ok(flow);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Flow newFlow)
        {
            await flowsService.CreateAsync(newFlow);

            return CreatedAtAction(nameof(Get), new { id = newFlow.Id }, newFlow);
        }

        [HttpPut("{id}")]
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