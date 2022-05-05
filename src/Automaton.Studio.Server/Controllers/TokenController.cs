using Automaton.Studio.Server.Core.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Automaton.Studio.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    public class TokenController : BaseController
    {
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshAccessToken(RefreshAccessTokenCommand command, CancellationToken ct)
        {
            return Ok(await Mediator.Send(command, ct));
        }

        [HttpPost]
        public async Task<IActionResult> RevokeAccessToken(RevokeAccessTokenCommand command, CancellationToken ct)
        {
            await Mediator.Send(command, ct);

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> RevokeRefreshToken(RevokeRefreshTokenCommand command, CancellationToken ct)
        {
            await Mediator.Send(command, ct);

            return NoContent();
        }
    }
}