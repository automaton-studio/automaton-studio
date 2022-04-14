using AuthServer.Core.Domains;
using Microsoft.AspNetCore.Identity;

namespace AuthServer.Core.Services
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
