using Automaton.Client.Auth.Interfaces;
using Automaton.Client.Auth.Models;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Automaton.Runner.Services;

public class AuthenticationStorage : IAuthenticationStorage
{
    private const string JsonWebToken = "jsonWebToken";

    private readonly App application = (App)Application.Current;

    public async Task<JsonWebToken> GetJsonWebToken()
    {
        return await Task.Run(() =>
        {
            var jsonWebToken = new JsonWebToken();

            if (application.Properties.Contains(JsonWebToken))
            {
                var jsonWebTokenProperty = application.Properties[JsonWebToken];
                jsonWebToken = JsonConvert.DeserializeObject<JsonWebToken>(jsonWebTokenProperty.ToString());
            }

            return jsonWebToken;
        });
    }

    public async Task SetJsonWebToken(JsonWebToken token)
    {
        await Task.Run(() => { application.Properties[JsonWebToken] = JsonConvert.SerializeObject(token); });
    }

    public async Task DeleteJsonWebToken()
    {
        await Task.Run(() =>
        {
            application.Properties.Remove(JsonWebToken);
        });
    }

    public async Task<string> GetAccessToken()
    {
        var jsonWebToken = await GetJsonWebToken();

        return await Task.Run(() => jsonWebToken != null ? jsonWebToken.AccessToken : string.Empty);
    }

    public async Task<string> GetRefreshToken()
    {
        var jsonWebToken = await GetJsonWebToken();

        return await Task.Run(() => jsonWebToken != null ? jsonWebToken.RefreshToken : string.Empty);
    }
}
