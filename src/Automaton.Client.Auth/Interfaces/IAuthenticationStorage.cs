using Automaton.Client.Auth.Models;

namespace Automaton.Client.Auth.Interfaces;

public interface IAuthenticationStorage
{
    Task<string> GetRefreshToken();
    Task<string> GetAuthToken();
    Task SetJsonWebToken(JsonWebToken token);
    JsonWebToken GetJsonWebToken();
    Task<JsonWebToken> GetJsonWebTokenAsync();
    Task DeleteJsonWebToken();
}
