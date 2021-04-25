using Automaton.Studio.Api.Models;
using Automaton.Studio.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Automaton.Studio.Api
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(JwtBearerDefaults.AuthenticationScheme)]
    public class RegisterController : ControllerBase
    {
        private const string UserIdClaim = "uid";

        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<TokenController> _logger;
        private readonly IRunnerService runnerService;

        public RegisterController(
            UserManager<IdentityUser> userManager,
            ILogger<TokenController> logger,
            IRunnerService runnerService)
        {
            this.runnerService = runnerService;
            _userManager = userManager;
            _logger = logger;
        }

        /// <summary>
        /// POST api/<RegisterController>
        /// </summary>
        [HttpPost]
        public ActionResult Post([FromBody] RegisterDetails details)
        {
            if (string.IsNullOrWhiteSpace(details.RunnerName))
            {
                return BadRequest("Invalid runner name.");
            }

            var runner = new Runner
            {
                Name = details.RunnerName,
                UserId = GetUserId()
            };

            if (runnerService.Exists(runner))
            {
                return BadRequest($"Runner already exists.");
            }

            runnerService.Create(runner);

            return Ok();
        }

        /// <summary>
        /// Retrieves connected user Id
        /// </summary>
        /// <returns>Connected user Id</returns>
        private string GetUserId()
        {
            var userIdClaim = User.Claims.SingleOrDefault(x => x.Type == UserIdClaim);

            if (userIdClaim is null)
                throw new ArgumentNullException("userId");

            return userIdClaim.Value;
        }
    }
}
