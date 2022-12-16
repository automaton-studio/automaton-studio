using Automaton.Studio.Server.Entities;
using Automaton.Studio.Server.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Automaton.Studio.Server.Services;

public class RoleManagerService
{
    private readonly RoleManager<ApplicationRole> _roleManager;

    public RoleManagerService(RoleManager<ApplicationRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task AddClaim(string roleName, RoleClaim claim)
    {
        await _roleManager.AddClaimAsync(await GetRole(roleName), new Claim(claim.ClaimType, claim.ClaimValue));
    }

    public async Task AddRole(ApplicationRole role)
    {
        await _roleManager.CreateAsync(role);
    }

    public async Task<IEnumerable<RoleClaim>> GetClaims(string roleName)
    {
        return (await _roleManager.GetClaimsAsync(await GetRole(roleName))).Select(c=>new RoleClaim() { ClaimType = c.ValueType, ClaimValue = c.Value });
    }

    public async Task<ApplicationRole> GetRole(string name)
    {
        return await _roleManager.FindByNameAsync(name);
    }

    public async Task<ApplicationRole> GetRole(Guid roleId)
    {
        return await _roleManager.FindByIdAsync(roleId.ToString());
    }

    public async Task RemoveClaim(string roleName, RoleClaim claim)
    {
        await _roleManager.RemoveClaimAsync(await GetRole(roleName), new Claim(claim.ClaimType, claim.ClaimValue));
    }

    public async Task RemoveRole(string name)
    {
        await _roleManager.DeleteAsync(await GetRole(name));
    }

    public async Task RemoveRole(Guid roleId)
    {
        await _roleManager.DeleteAsync(await GetRole(roleId));
    }
}
