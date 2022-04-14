using System.Threading;
using System.Threading.Tasks;
using AuthServer.Core.Commands;
using Common.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Automaton.Studio.Server.Controllers
{
    public class AccountController : BaseController
    {
        /// <summary>
        /// Registers new user
        /// </summary>
        /// <param name="registerUserCommand">Information for registering a new user</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns>User fetch URL in headers</returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserCommand registerUserCommand,
            CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await Mediator.Send(registerUserCommand, ct);
            return CreatedAtRoute("Default", new {controller = "User", userId = registerUserCommand.Id},
                registerUserCommand.Id);
        }

        /// <summary>
        /// Registers new user
        /// </summary>
        /// <param name="signInUserCommand">Information for authenticating a user</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns>JsonWebToken</returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<JsonWebToken>> LoginUser([FromBody] SignInUserCommand signInUserCommand,
            CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return await Mediator.Send(signInUserCommand, ct);
        }

        /// <summary>
        /// Updates user password
        /// </summary>
        /// <param name="passwordUpdateCommand">User password update details</param>
        /// <returns>Empty OK response</returns>
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateUserPassword([FromBody] UpdateUserPasswordCommand passwordUpdateCommand)
        {
            await Mediator.Send(passwordUpdateCommand);
            return NoContent();
        }
    }
}