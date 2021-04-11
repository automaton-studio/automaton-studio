using System.Threading.Tasks;

namespace Automaton.Common.Auth
{
    public interface IAccessTokenManagerService
    {
        Task<bool> CurrentAccessTokenIsActive();
        Task DeactivateCurrentAccessTokenAsync();
        Task<bool> CurrentAccessTokenIsActive(string token);
        Task DeactivateAccessTokenAsync(string token);
    }
}