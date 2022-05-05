using AuthServer.Core.Domains;

namespace Automaton.Studio.Server.Services.Interfaces
{
    public interface IRoleManagerService
    {
        Task AddRole(ApplicationRole role);
        Task<ApplicationRole> GetRole(String name);
        Task<ApplicationRole> GetRole(Guid roleId);
        Task RemoveRole(String name);
        Task RemoveRole(Guid roleId);
        Task AddClaim(String roleName, RoleClaim claim);
        Task RemoveClaim(String roleName, RoleClaim claim);
        Task<IEnumerable<RoleClaim>> GetClaims(string roleName);
    }
}
