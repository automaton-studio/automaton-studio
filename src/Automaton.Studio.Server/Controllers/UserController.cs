using AuthServer.Core.Queries;
using Automaton.Studio.Server.Core.Commands;
using Automaton.Studio.Server.Models;
using Common.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Automaton.Studio.Server.Controllers;

[Route("api/[controller]/[action]")]
public class UserController : BaseController
{
    [HttpGet]
    public async Task<ActionResult<UserDetails>> GetUserProfile()
    {
        return Ok(await Mediator.Send(new UserQuery { Id = GetUserId() }));
    }

    /// <summary>
    /// Get user profile for the ID
    /// </summary>
    /// <param name="userId">Unique user identifier</param>
    /// <returns>User profile data</returns>
    [HttpGet("{userId}", Name = "User")]
    [Authorize(Policy = "Admin")]
    public async Task<ActionResult<UserDetails>> FindUserById([FromRoute, NotEmptyGuid] Guid userId)
    {
        return Ok(await Mediator.Send(new UserQuery {Id = userId}));
    }

    /// <summary>
    /// Query for user profile
    /// </summary>
    /// <param name="filterUserQuery">Query filter values</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Collection of user profiles</returns>
    [HttpGet]
    [Authorize(Policy = "Admin")]
    public async Task<ActionResult<IEnumerable<UserDetails>>> FindUsers([FromQuery] FilterUserQuery filterUserQuery, CancellationToken ct)
    {
        return Ok(await Mediator.Send(filterUserQuery, ct));
    }

    /// <summary>
    /// Updates user profile details
    /// </summary>
    /// <param name="profileUpdateCommand">User profile details</param>
    /// <returns>Empty OK response</returns>
    [HttpPut("{userId}")]
    [Authorize(Policy = "Admin")]
    public Task UpdateUserInfo([FromBody] UpdateUserInfoCommand profileUpdateCommand)
    {
        return Mediator.Send(profileUpdateCommand);
    }

    /// <summary>
    /// Updates user password
    /// </summary>
    /// <param name="passwordUpdateCommand">User password update details</param>
    /// <returns>Empty OK reponse</returns>
    [HttpPut("{userId}/password")]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> UpdateUserPassword([FromBody] UpdateUserPasswordCommand passwordUpdateCommand)
    {
        await Mediator.Send(passwordUpdateCommand);
        return NoContent();
    }

    /// <summary>
    /// Get all roles for user
    /// </summary>  
    /// <param name="userId">Unique user identifier</param>
    /// <returns>List of role names user belongs to</returns>
    [HttpGet("{userId}/roles", Name = "Roles")]
    [Authorize(Policy = "Admin")]
    public virtual async Task<ActionResult<IEnumerable<string>>> GetUserRoles([FromRoute, NotEmptyGuid] Guid userId)
    {
        var data = await Mediator.Send(new UserRolesQuery {UserId = userId});
        return Ok(data);
    }

    /// <summary>
    /// Removes user from the role
    /// </summary>
    /// <param name="userId">Unique user identifier</param>
    /// <param name="roleName">Role name</param>
    /// <returns>No content 204 status code</returns>
    [HttpDelete("{userId}/roles/{roleName}")]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> RemoveUserRole([FromRoute] Guid userId, [FromRoute, Required] string roleName)
    {
        await Mediator.Send(new RemoveUserRoleCommand(userId, roleName));
        return NoContent();
    }

    /// <summary>
    /// Add user to the role
    /// </summary>
    /// <param name="userId">Unique user identifier</param>
    /// <param name="roleName">Role name</param>
    /// <returns>No content 204 status code</returns>
    [HttpPut("{userId}/roles/{roleName}")]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> AddUserRole([FromRoute, NotEmptyGuid] Guid userId,
        [FromRoute, Required] String roleName)
    {
        await Mediator.Send(new AddUserRoleCommand(userId, roleName));
        return NoContent();
    }
}