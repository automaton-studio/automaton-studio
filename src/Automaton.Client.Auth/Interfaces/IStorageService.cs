using Automaton.Client.Auth.Models;

namespace Automaton.Client.Auth.Interfaces
{
    public interface IStorageService
    {
        Task<string> GetRefreshToken();
        Task<string> GetAuthToken();
        Task SetAuthAndRefreshTokens(JsonWebToken token);
        Task DeleteAuthAndRefreshTokens();
    }
}
