using Automaton.Studio.Server.Core.Commands;
using Automaton.Studio.Server.Models;
using Automaton.Studio.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace Automaton.Studio.Server.Controllers
{
    public class RunnersController : BaseController
    {
        private readonly RunnerService runnersService;

        public RunnersController(RunnerService runnersService)
        {
            this.runnersService = runnersService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Runner>> Get()
        {
            return Ok(runnersService.List());
        }

        [HttpGet("byname/{name}")]
        public ActionResult<Runner> GetByName(string name)
        {
            return Ok(runnersService.GetRunnerByName(name));
        }

        [HttpGet("find")]
        public async Task<ActionResult<IEnumerable<Runner>>> Get(IEnumerable<Guid> runnerIds)
        {
            return Ok(runnersService.List(runnerIds));
        }

        [HttpPost("register")]
        public ActionResult Post([FromBody] RegisterRunnerDetails registerRunnerDetails, CancellationToken cancellationToken)
        {
            return Ok(runnersService.SetupRunnerDetails(registerRunnerDetails));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, UpdateRunnerDetails runnerDetails, CancellationToken cancellationToken)
        {
            await runnersService.UpdateRunnerDetails(id, runnerDetails, cancellationToken);

            return NoContent();
        }
    }
}
