using Automaton.Studio.Server.Models;
using Automaton.Studio.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace Automaton.Studio.Server.Controllers;

public class LogsController : BaseController
{
    private readonly LogsService logsService;
    private readonly ILogger<LogsController> logger;

    public LogsController(LogsService logsService, ILogger<LogsController> logger)
    {
        this.logsService = logsService;
        this.logger = logger;
    }

    [HttpGet]
    public IEnumerable<LogMessage> GetLogs()
    {
        return logsService.List();
    }

    [HttpGet("{id}")]
    public LogMessage Get(Guid id)
    {
        var log = logsService.Get(id);

        return log;
    }

    [HttpPut("{id}")]
    public IActionResult Put(Guid id, LogMessage log)
    {
        logsService.Update(id, log);

        return NoContent();
    }

    [HttpPost]
    public IActionResult Post(LogMessage log)
    {
        var logId = logsService.Create(log);

        var newLog = logsService.Get(logId);

        return CreatedAtAction(nameof(Get), new { id = newLog.Id }, newLog);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        logsService.Remove(id);

        return NoContent();
    }
}
