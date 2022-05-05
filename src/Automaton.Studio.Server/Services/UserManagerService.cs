using AuthServer.Core.Domains;
using Automaton.Studio.Server.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Automaton.Studio.Server.Services
{
    public class UserManagerService : IUserManagerService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public UserManagerService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task CreateUser(ApplicationUser user, string password)
        {
            if (_userManager.Users.Any(u => u.UserName == user.UserName))
            {
                throw new Exception($"User with user name {user.UserName} already registered");
            }

            if (_userManager.Users.Any(u => u.Email == user.Email))
            {
                throw new Exception($"User with email address {user.Email} already registered");
            }

            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                var exceptionText = result.Errors.Aggregate(
                    "User Creation Failed - Identity Exception. Errors were: \n\r\n\r",
                    (current, error) => current + (" - " + error.Description + "\n\r"));
                throw new Exception(exceptionText);
            }
        }

        public async Task UpdatePassword(Guid userId, string oldPassword, string newPassword)
        {
            var user = await GetUserById(userId);
            var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            if (!result.Succeeded)
            {
                throw new Exception("Error in updating password!");
            }
        }

        public async Task UpdateProfile(Guid userId, string firstName, string lastName)
        {
            var user = await GetUserById(userId);
            user.FirstName = firstName;
            user.LastName = lastName;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                throw new Exception("Error in updating Profile!");
            }
        }

        public async Task<ApplicationUser> GetUserById(Guid userId)
        {
            var result = await _userManager.FindByIdAsync(userId.ToString());
            if (result == null)
                throw new Exception($"User with userId {userId} not found!");

            return result;
        }

        public async Task<ApplicationUser> GetUserByEmailOrUserName(string emailOrUserName)
        {
            var result = await _userManager.FindByEmailAsync(emailOrUserName) ??
                         await _userManager.FindByNameAsync(emailOrUserName);
            if (result == null)
                throw new Exception($"User with Email or UserName '{emailOrUserName}' not found!");

            return result;
        }

        public async Task<bool> ValidateCredentialsAsync(string usernameOrEmail, string password)
        {
            var user = await _userManager.FindByEmailAsync(usernameOrEmail) ??
                       await _userManager.FindByNameAsync(usernameOrEmail);

            if (user != null)
            {
                return await _userManager.CheckPasswordAsync(user, password);
            }

            return false;
        }

        public async Task<bool> ValidatePasswordAsync(ApplicationUser user, string password)
        {
            if (user != null)
            {
                return await _userManager.CheckPasswordAsync(user, password);
            }

            return false;
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsers(string firstName, string lastName, string email,
            int pageindex, int pageSize)
        {
            await Task.CompletedTask;
            var result = _userManager.Users.Where(
                    u =>
                        (firstName == null || firstName == "" || u.FirstName.StartsWith(firstName)) &&
                        (lastName == null || lastName == "" || u.LastName.StartsWith(lastName)) &&
                        (email == null || email == "" || u.FirstName == firstName)
                )
                .Skip(pageindex * pageSize)
                .Take(pageSize);

            return result;
        }

        public async Task<string> GetResetPasswordToken(Guid userId)
        {
            var user = await GetUserById(userId);
            if (user != null)
            {
                return await _userManager.GeneratePasswordResetTokenAsync(user);
            }

            throw new Exception();
        }

        public async Task ResetPassword(Guid userId, string token, string password)
        {
            var user = await GetUserById(userId);
            if (user != null)
            {
                var result = await _userManager.ResetPasswordAsync(user, token, password);
                if (!result.Succeeded)
                {
                    throw new Exception($"Error in resetting password");
                }
            }

            throw new Exception($"User with userId {userId} not found!");
        }

        public async Task<IEnumerable<string>> GetRoles(Guid userId)
        {
            return await _userManager.GetRolesAsync(await GetUserById(userId));
        }

        public async Task AddRole(Guid userId, string roleName)
        {
            var result = await _userManager.AddToRoleAsync(await GetUserById(userId), roleName);
            if (!result.Succeeded)
            {
                throw new Exception("Error in Adding Role!");
            }
        }

        public async Task AddRole(Guid userId, Guid roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            var result = await _userManager.AddToRoleAsync(await GetUserById(userId), role.Name);
            if (!result.Succeeded)
            {
                throw new Exception("Error in Adding Role!");
            }
        }

        public async Task RemoveRole(Guid userId, string roleName)
        {
            var result = await _userManager.RemoveFromRoleAsync(await GetUserById(userId), roleName);
            if (!result.Succeeded)
            {
                throw new Exception("Error in Removing Role!");
            }
        }

        public async Task RemoveRole(Guid userId, Guid roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            var result = await _userManager.RemoveFromRoleAsync(await GetUserById(userId), role.Name);
            if (!result.Succeeded)
            {
                throw new Exception("Error in Removing Role!");
            }
        }
    }
}