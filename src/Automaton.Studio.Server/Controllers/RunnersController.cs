using Automaton.Studio.Server.Core.Commands;
using Automaton.Studio.Server.Models;
using Microsoft.AspNetCore.Mvc;
using AuthServer.Core.Queries;

namespace Automaton.Studio.Server.Controllers
{
    public class RunnersController : BaseController
    {        
        [HttpPost("register")]
        public async Task<ActionResult> Post([FromBody] RegisterRunnerCommand command, CancellationToken cancellationToken)
        {
            return Ok(await Mediator.Send(command, cancellationToken));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Runner>>> Get([FromQuery] RunnerQuery runnerQuery, CancellationToken cancellationToken)
        {
            return Ok(await Mediator.Send(runnerQuery, cancellationToken));
        }
    }
}
