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
    //[Authorize(JwtBearerDefaults.AuthenticationScheme)]
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
        public IEnumerable<Flow> Get()
        {
            return flowsService.Get();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Flow>> Get(Guid id)
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
        public async Task<IActionResult> Put(Guid id, Flow flow)
        {
            await flowsService.UpdateAsync(id, flow);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await flowsService.RemoveAsync(id);

            return NoContent();
        }
    }
}