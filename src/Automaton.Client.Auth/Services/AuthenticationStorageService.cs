using Automaton.Client.Auth.Interfaces;
using Automaton.Client.Auth.Models;
using Blazored.LocalStorage;

namespace Automaton.Client.Auth.Services
{
    public class AuthenticationStorageService : IAuthenticationStorage
    {
        private const string JsonWebToken = "jsonWebToken";

        private readonly ILocalStorageService localStorage;

        public AuthenticationStorageService
        (
           ILocalStorageService localStorage
        )
        {
            this.localStorage = localStorage;
        }

        public async Task<string> GetRefreshToken()
        {
            var jsonWebToken = await localStorage.GetItemAsync<JsonWebToken>(JsonWebToken);
            var refreshToken = jsonWebToken != null ? jsonWebToken.RefreshToken : string.Empty;

            return refreshToken;
        }

        public async Task<string> GetAccessToken()
        {
            var jsonWebToken = await localStorage.GetItemAsync<JsonWebToken>(JsonWebToken);
            var authToken = jsonWebToken != null ? jsonWebToken.AccessToken : string.Empty;

            return authToken;
        }

        public async Task SetJsonWebToken(JsonWebToken token)
        {
            await localStorage.SetItemAsync(JsonWebToken, token);
        }

        public async Task<JsonWebToken> GetJsonWebToken()
        {
            var jsonWebToken = await localStorage.GetItemAsync<JsonWebToken>(JsonWebToken);

            return jsonWebToken;
        }

        public async Task DeleteJsonWebToken()
        {
            await localStorage.RemoveItemAsync(JsonWebToken);
        }  
    }
}
