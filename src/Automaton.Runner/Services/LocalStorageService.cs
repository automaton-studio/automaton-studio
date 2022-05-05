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
            var refreshToken = application.Properties[RefreshToken].ToString();

            return await Task.FromResult(refreshToken);
        }

        public async Task<string> GetAuthToken()
        {
            var refreshToken = application.Properties[AuthToken].ToString();

            return await Task.FromResult(refreshToken);
        }

        public async Task SetAuthAndRefreshTokens(JsonWebToken token)
        {
            await Task.FromResult(() =>
            {
                application.Properties[AuthToken] = token.AccessToken;
                application.Properties[RefreshToken] = token.RefreshToken;
            });
        }

        public async Task DeleteAuthAndRefreshTokens()
        {
            await Task.FromResult(() =>
            {
                application.Properties.Remove(AuthToken);
                application.Properties.Remove(RefreshToken);
            });
        }
    }
}
