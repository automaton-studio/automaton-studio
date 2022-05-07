using Automaton.Client.Auth.Interfaces;
using Automaton.Client.Auth.Models;
using System.Threading.Tasks;
using System.Windows;

namespace Automaton.Runner.Services
{
    public class LocalStorageService : IStorageService
    {
        private const string AuthToken = "authToken";
        private const string RefreshToken = "refreshToken";

        private readonly App application = (App)Application.Current;

        public async Task<string> GetRefreshToken()
        {
            var refreshToken = application.Properties.Contains(RefreshToken) ?
                application.Properties[RefreshToken].ToString()
                : string.Empty;

            return await Task.Run(() => refreshToken);
        }

        public async Task<string> GetAuthToken()
        {
            var authToken = application.Properties.Contains(AuthToken) ? 
                application.Properties[AuthToken].ToString()
                : string.Empty;

            return await Task.Run(() => authToken);
        }

        public async Task SetJsonWebToken(JsonWebToken token)
        {
            await Task.Run(() =>
            {
                application.Properties[AuthToken] = token.AccessToken;
                application.Properties[RefreshToken] = token.RefreshToken;
            });
        }

        public async Task DeleteJsonWebToken()
        {
            await Task.Run(() =>
            {
                application.Properties.Remove(AuthToken);
                application.Properties.Remove(RefreshToken);
            });
        }
    }
}
