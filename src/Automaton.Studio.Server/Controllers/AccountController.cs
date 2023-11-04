using Automaton.Studio.Server.Core.Commands;
using Automaton.Studio.Server.Data;
using Automaton.Studio.Server.Entities;
using Automaton.Studio.Server.Services;
using Common.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Automaton.Studio.Server.Controllers;

[Route("api/[controller]/[action]")]
public class AccountController : BaseController
{
    private readonly ConfigurationService configurationService;
    private readonly ApplicationDbContext _dataContext;
    private readonly UserManagerService _userManagerService;

    public AccountController(ConfigurationService configurationService,
        ApplicationDbContext dataContext, 
        UserManagerService userManagerService)
    {
        this.configurationService = configurationService;
        _dataContext = dataContext;
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

        var signInUserQuery = new SignInUserQuery(configurationService.RefreshTokenLifetime, signInUserCommand.UserName, signInUserCommand.Password);

        return Ok(await Mediator.Send(signInUserQuery, cancellationToken));
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<JsonWebToken>> LoginRunnerUser(SignInUserCommand signInUserCommand, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var signInUserQuery = new SignInUserQuery(configurationService.RunnerRefreshTokenLifetime, signInUserCommand.UserName, signInUserCommand.Password);

        return Ok(await Mediator.Send(signInUserQuery, cancellationToken));
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
}