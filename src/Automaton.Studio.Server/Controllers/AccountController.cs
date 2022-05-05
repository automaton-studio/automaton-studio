using Automaton.Studio.Server.Core.Commands;
using Common.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Automaton.Studio.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AccountController : BaseController
    {
        /// <summary>
        /// Registers new user
        /// </summary>
        /// <param name="registerUserCommand">Information for registering a new user</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>User fetch URL in headers</returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserCommand registerUserCommand, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await Mediator.Send(registerUserCommand, cancellationToken);

            return CreatedAtRoute("User", 
                new {controller = "User", userId = registerUserCommand.Id},
                registerUserCommand.Id);
        }

        /// <summary>
        /// Registers new user
        /// </summary>
        /// <param name="signInUserCommand">Information for authenticating a user</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>JsonWebToken</returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<JsonWebToken>> LoginUser([FromBody] SignInUserCommand signInUserCommand, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return await Mediator.Send(signInUserCommand, cancellationToken);
        }

        /// <summary>
        /// Updates user password
        /// </summary>
        /// <param name="passwordUpdateCommand">User password update details</param>
        /// <returns>Empty OK response</returns>
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateUserPassword([FromBody] UpdateUserPasswordCommand passwordUpdateCommand, CancellationToken cancellationToken)
        {
            await Mediator.Send(passwordUpdateCommand, cancellationToken);

            return NoContent();
        }
    }
}