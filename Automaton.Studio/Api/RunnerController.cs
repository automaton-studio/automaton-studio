using Automaton.Studio.Api.Models;
using Automaton.Studio.Entities;
using Automaton.Studio.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Automaton.Studio.Api
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(JwtBearerDefaults.AuthenticationScheme)]
    public class RunnerController : ControllerBase
    {
        private const string UserIdClaim = "uid";

        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<TokenController> _logger;
        private readonly IRunnerService runnerService;

        public RunnerController(
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
        [HttpPost("register")]
        public ActionResult Post([FromBody] RegisterDetails details)
        {
            if (string.IsNullOrWhiteSpace(details.RunnerName))
            {
                return BadRequest("Invalid runner name.");
            }

            if (runnerService.Exists(details.RunnerName))
            {
                return BadRequest($"Runner already exists.");
            }

            runnerService.Create(details.RunnerName);

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
