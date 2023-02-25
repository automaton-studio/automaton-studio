using Automaton.Studio.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace Automaton.Studio.Server.Controllers;

public class CustomStepsController : BaseController
{
    private readonly CustomStepsService customStepsService;

    public CustomStepsController(CustomStepsService customStepsService)
    {
        this.customStepsService = customStepsService;
    }

    [HttpGet]
    public IEnumerable<Models.CustomStep> Get()
    {
        return customStepsService.List();
    }

    [HttpGet("{id}")]
    public ActionResult<Models.CustomStep> Get(Guid id)
    {
        var flow = customStepsService.Get(id);

        if (flow is null)
        {
            return NotFound();
        }

        return Ok(flow);
    }

    [HttpPost]
    public IActionResult Post(Models.CustomStep customStep)
    {
        var customStepId = customStepsService.Create(customStep);

        var newCustomStep = customStepsService.Get(customStepId);

        return CreatedAtAction(nameof(Get), new { id = newCustomStep.Id }, newCustomStep);
    }

    [HttpPut("{id}")]
    public IActionResult Put(Guid id, Models.CustomStep customStep)
    {
        customStepsService.Update(id, customStep);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        customStepsService.Remove(id);

        return NoContent();
    }

    [HttpGet("exists/{name}")]
    public ActionResult<Models.CustomStep> Exists(string name)
    {
        var exists = customStepsService.Exists(name);

        return exists ? Ok(exists) : NotFound();
    }
}