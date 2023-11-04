using Automaton.Studio.Server.Core.Commands;
using Automaton.Studio.Server.Data;
using Automaton.Studio.Server.Services;
using Common.EF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Authentication;

namespace Automaton.Studio.Server.Controllers;

[Route("api/[controller]/[action]")]
public class TokenController : BaseController
{
    private readonly ConfigurationService configurationService;

    public TokenController(ConfigurationService configurationService)
    {
        this.configurationService = configurationService;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshAccessToken(RefreshAccessTokenCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var refreshAccessTokenQuery = new RefreshAccessTokenQuery(configurationService.RefreshTokenLifetime, command.Token);

            return Ok(await Mediator.Send(refreshAccessTokenQuery, cancellationToken));
        }
        catch (AuthenticationException)
        {
            return Unauthorized();
        }          
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshRunnerAccessToken(RefreshAccessTokenCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var refreshAccessTokenQuery = new RefreshAccessTokenQuery(configurationService.RunnerRefreshTokenLifetime, command.Token);

            return Ok(await Mediator.Send(refreshAccessTokenQuery, cancellationToken));
        }
        catch (AuthenticationException)
        {
            return Unauthorized();
        }
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