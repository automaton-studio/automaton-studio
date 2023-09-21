using Automaton.Studio.Server.Entities;
using Automaton.Studio.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace Automaton.Studio.Server.Controllers;

public class LogsController : BaseController
{
    private readonly LogsService logsService;

    public LogsController(LogsService logsService)
    {
        this.logsService = logsService;
    }

    //[HttpGet]
    //public IEnumerable<Log> Get()
    //{
    //    return logsService.List();
    //}

    //[HttpGet("{id}")]
    //public ActionResult<Models.CustomStep> Get(Guid id)
    //{
    //    var flow = logsService.Get(id);

    //    if (flow is null)
    //    {
    //        return NotFound();
    //    }

    //    return Ok(flow);
    //}

    //[HttpPost]
    //public IActionResult Post(Models.NewCustomStep customStep)
    //{
    //    var customStepId = logsService.Create(customStep);

    //    var newCustomStep = logsService.Get(customStepId);

    //    return CreatedAtAction(nameof(Get), new { id = newCustomStep.Id }, newCustomStep);
    //}

    //[HttpPut("{id}")]
    //public IActionResult Put(Guid id, Models.CustomStep customStep)
    //{
    //    logsService.Update(id, customStep);

    //    return NoContent();
    //}

    //[HttpDelete("{id}")]
    //public IActionResult Delete(Guid id)
    //{
    //    logsService.Remove(id);

    //    return NoContent();
    //}

    //[HttpGet("exists/{name}")]
    //public ActionResult<Models.CustomStep> Exists(string name)
    //{
    //    var exists = logsService.Exists(name);

    //    return exists ? Ok(exists) : NotFound();
    //}
}