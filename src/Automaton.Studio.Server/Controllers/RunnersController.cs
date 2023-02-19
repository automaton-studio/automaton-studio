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

        [HttpPost("register")]
        public ActionResult Post([FromBody] RegisterRunnerDetails registerRunnerDetails, CancellationToken cancellationToken)
        {
            return Ok(runnersService.Create(registerRunnerDetails));
        }

        [HttpGet]
        public ActionResult<IEnumerable<Runner>> Get()
        {
            return Ok(runnersService.List());
        }

        [HttpGet("find")]
        public async Task<ActionResult<IEnumerable<Runner>>> Get(IEnumerable<Guid> runnerIds)
        {
            return Ok(runnersService.List(runnerIds));
        }
    }
}
