using Automaton.Client.Auth.Interfaces;
using Automaton.Client.Auth.Models;
using System.Threading.Tasks;
using System.Windows;

namespace Automaton.Runner.Services
{
    public class LocalStorageService : IStorageService
    {
        private const string JsonWebToken = "jsonWebToken";

        private readonly App application = (App)Application.Current;

        public async Task<string> GetRefreshToken()
        {
            var jsonWebToken = await GetJsonWebToken();

            return await Task.Run(() => jsonWebToken != null ? jsonWebToken.RefreshToken : string.Empty);
        }

        public async Task<string> GetAuthToken()
        {
            var jsonWebToken = await GetJsonWebToken();

            return await Task.Run(() => jsonWebToken != null ? jsonWebToken.AccessToken : string.Empty);

        }

        public async Task SetJsonWebToken(JsonWebToken token)
        {
            await Task.Run(() =>
            {
                application.Properties[JsonWebToken] = token;
            });
        }

        public async Task DeleteJsonWebToken()
        {
            await Task.Run(() =>
            {
                application.Properties.Remove(JsonWebToken);
            });
        }

        public async Task<JsonWebToken> GetJsonWebToken()
        {
            var jsonWebToken = application.Properties.Contains(JsonWebToken) ?
                application.Properties[JsonWebToken]
                : null;

            return await Task.Run(() => jsonWebToken as JsonWebToken);
        }
    }
}
