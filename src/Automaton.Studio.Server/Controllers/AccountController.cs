using AuthServer.Core.Events;
using AutoMapper;
using Automaton.Studio.Server.Core.Commands;
using Automaton.Studio.Server.Entities;
using Automaton.Studio.Server.Services;
using Azure.Core;
using Common.Authentication;
using Common.EF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Immutable;

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
        private readonly IJwtService _jwtService;


        public AccountController(ConfigurationService configurationService, 
            IDataContext dataContext, IMapper mapper,
            ILogger<AccountController> logger,
            UserManagerService userManagerService,
            IJwtService jwtService)
        {
            this.configurationService = configurationService;
            _jwtService = jwtService;
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
        public async Task<ActionResult<JsonWebToken>> LoginUser(SignInUserCommand signInUserCommand, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManagerService.GetUserByEmailOrUserName(signInUserCommand.UserName);

            if (user == null || await _userManagerService.ValidatePasswordAsync(user, signInUserCommand.Password) == false)
            {
                throw new Exception("Invalid credentials.");
            }

            var refreshToken = new RefreshToken<Guid>(user.Id, 4);
            var roles = (await _userManagerService.GetRoles(user.Id)).ToImmutableList();
            var jwt = _jwtService.GenerateToken(user.Id.ToString(), user.UserName, roles, GetCustomClaimsForUser(user.Id));

            jwt.RefreshToken = refreshToken.Token;

            await _dataContext.Set<RefreshToken<Guid>>().AddAsync(refreshToken, cancellationToken);
            await _dataContext.SaveChangesAsync(cancellationToken);

            return jwt;
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUserPassword(UpdateUserPasswordCommand passwordUpdateCommand, CancellationToken cancellationToken)
        {
            var userId = GetUserId();

            await _userManagerService.UpdatePassword
            (
                userId, 
                passwordUpdateCommand.OldPassword, 
                passwordUpdateCommand.NewPassword
            );

            await _dataContext.SaveChangesAsync(cancellationToken);

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

        private IDictionary<string, string> GetCustomClaimsForUser(Guid userId)
        {
            //Add custom claims here
            return new Dictionary<string, string>();
        }
    }
}