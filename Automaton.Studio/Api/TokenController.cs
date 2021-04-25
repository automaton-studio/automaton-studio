using Automaton.Common.Auth;
using Automaton.Studio.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Automaton.Studio.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IJwtService _jwtService;
        private readonly ILogger<TokenController> _logger;

        public TokenController(SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            ILogger<TokenController> logger,
            IJwtService jwtService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
            _logger = logger;
        }

        /// <summary>
        /// POST api/<TokenController>
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] SignInUserDetails userDetails)
        {
            if (string.IsNullOrWhiteSpace(userDetails.UserName) || 
                string.IsNullOrWhiteSpace(userDetails.Password))
            {
                return BadRequest("Invalid credentials.");
            }

            var result = await _signInManager.PasswordSignInAsync(userDetails.UserName, userDetails.Password, isPersistent: true, lockoutOnFailure: false);
            
            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in.");

                IdentityUser user = await _userManager.FindByNameAsync(userDetails.UserName);

                if (user == null)
                {
                    return Unauthorized("Invalid credentials.");
                }

                var refreshToken = new RefreshToken<string>(user.Id, 4);
                var jwt = _jwtService.GenerateToken(user.Id.ToString(), user.UserName, GetCustomClaimsForUser(user.Id));

                jwt.RefreshToken = refreshToken.Token;
                
                // TODO! Implement RefreshTokens
                //await _dataContext.Set<RefreshToken<Guid>>().AddAsync(refreshToken, cancellationToken);
                //await _dataContext.SaveChangesAsync(cancellationToken);
                //await _mediator.Publish(new UserLoggedInEvent(user.Id), cancellationToken);

                _logger.Log(LogLevel.Debug, "UserLoggedIn Event Published.");

                return Ok(jwt);
            }

            if (result.IsLockedOut)
            {
                _logger.LogWarning("User account locked out.");

                return Unauthorized("User account locked out.");
            }
            
            _logger.LogWarning("Invalid login attempt.");

            return Unauthorized("Invalid login attempt.");
        }

        private static IDictionary<string, string> GetCustomClaimsForUser(string userId)
        {
            // Add custom claims here
            return new Dictionary<string, string>
            {
                { "uid", userId }
            };
        }
    }
}
