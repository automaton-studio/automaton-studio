using Automaton.Studio.Server.Core.Commands;
using Microsoft.AspNetCore.Mvc;

namespace Automaton.Studio.Server.Controllers
{
    public class RunnerController : BaseController
    {        
        [HttpPost("register")]
        public async Task<ActionResult> Post([FromBody] RegisterRunnerCommand command, CancellationToken cancellationToken)
        {
            return Ok(await Mediator.Send(command, cancellationToken));
        }

        [HttpGet("exists")]
        public async Task<ActionResult> Get([FromBody] RegisterRunnerCommand command, CancellationToken cancellationToken)
        {
            return Ok(await Mediator.Send(command, cancellationToken));
        }
    }
}
