using AuthServer.Core.Events;
using AutoMapper;
using Automaton.Studio.Server.Application.Commands.Handlers;
using Automaton.Studio.Server.Core.Commands;
using Automaton.Studio.Server.Entities;
using Automaton.Studio.Server.Services;
using Common.Authentication;
using Common.EF;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Automaton.Studio.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AccountController : BaseController
    {
        private readonly ConfigurationService configurationService;
        private readonly IDataContext _dataContext;
        private readonly UserManagerService _userManagerService;
        private readonly IMapper _mapper;
        private readonly ILogger<AccountController> _logger;

        public AccountController(ConfigurationService configurationService, 
            IDataContext dataContext, IMapper mapper,
            ILogger<AccountController> logger,
            UserManagerService userManagerService)
        {
            this.configurationService = configurationService;
            _dataContext = dataContext;
            _mapper = mapper;
            _logger = logger;
            _userManagerService = userManagerService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUser(RegisterUserCommand registerUserCommand)
        {
            if (configurationService.NoUserSignUp)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _userManagerService.CreateUser(new ApplicationUser
            {
                Id = Guid.NewGuid(),
                FirstName = registerUserCommand.FirstName,
                LastName = registerUserCommand.LastName,
                UserName = registerUserCommand.UserName,
                Email = registerUserCommand.Email,
                SecurityStamp = Guid.NewGuid().ToString()
            }, registerUserCommand.Password);

            await _dataContext.SaveChangesAsync();

            return CreatedAtRoute("User",
                new { controller = "User", userId = registerUserCommand.UserName },
                registerUserCommand.UserName);
        }

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

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateUserPassword(UpdateUserPasswordCommand passwordUpdateCommand, CancellationToken cancellationToken)
        {
            await Mediator.Send(passwordUpdateCommand, cancellationToken);

            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUserProfile(UpdateUserInfoCommand profileUpdateCommand, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            profileUpdateCommand.Id = GetUserId();

            await Mediator.Send(profileUpdateCommand, cancellationToken);

            return NoContent();
        }
    }
}