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

        [HttpGet("find")]
        public ActionResult<IEnumerable<Runner>> Get(IEnumerable<Guid> runnerIds)
        {
            return Ok(runnersService.List(runnerIds));
        }

        [HttpPost]
        public ActionResult Post(Runner runner, CancellationToken cancellationToken)
        {
            return Ok(runnersService.Add(runner));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, Runner runnerDetails, CancellationToken cancellationToken)
        {
            await runnersService.Update(id, runnerDetails, cancellationToken);

            return NoContent();
        }
    }
}
