using Automaton.Core.Models;
using Automaton.Studio.Server.Models;
using Automaton.Studio.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace Automaton.Studio.Server.Controllers;

public class FlowsController : BaseController
{
    private readonly FlowsService flowsService;

    public FlowsController(FlowsService flowsService)
    {
        this.flowsService = flowsService;
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
    public ActionResult Post(ExecuteFlowDetails command)
    {
        return Ok(flowsService.Execute(command.FlowId, command.RunnerIds));
    }

    [HttpGet("exists/{name}")]
    public ActionResult<Flow> Exists(string name)
    {
        var exists = flowsService.Exists(name);

        return exists ? Ok(exists) : NotFound();
    }
}