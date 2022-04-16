using AuthServer.Core.Domains;

namespace AuthServer.Core.Services
{
    public interface IUserManagerService
    {
        Task CreateUser(ApplicationUser user, string password);
        Task UpdatePassword(Guid userId, string oldPassword, string newPassword);
        Task<string> GetResetPasswordToken(Guid userId);
        Task ResetPassword(Guid userId, string token, string password);
        Task UpdateProfile(Guid userId, string firstName, string lastName);
        Task<ApplicationUser> GetUserById(Guid userId);
        Task<ApplicationUser> GetUserByEmailOrUserName(string emailOrUserName);
        Task<bool> ValidateCredentialsAsync(string usernameOrEmail, string password);
        Task<bool> ValidatePasswordAsync(ApplicationUser user, string password);
        Task<IEnumerable<ApplicationUser>> GetUsers(string firstName, string lastName, string email, int pageindex,
            int pageSize);
        Task<IEnumerable<string>> GetRoles(Guid userId);
        Task AddRole(Guid userId, string roleName);
        Task AddRole(Guid userId, Guid roleId);
        Task RemoveRole(Guid userId, string roleName);
        Task RemoveRole(Guid userId, Guid roleId);
    }
}