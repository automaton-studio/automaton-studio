using Automaton.Client.Auth.Models;

namespace Automaton.Client.Auth.Interfaces
{
    public interface IAuthenticationStorage
    {
        Task<string> GetRefreshToken();
        Task<string> GetAuthToken();
        Task SetJsonWebToken(JsonWebToken token);
        Task<JsonWebToken> GetJsonWebToken();
        Task DeleteJsonWebToken();
    }
}
