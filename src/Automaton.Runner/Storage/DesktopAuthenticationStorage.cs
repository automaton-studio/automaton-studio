using Automaton.Client.Auth.Interfaces;
using Automaton.Client.Auth.Models;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Automaton.Runner.Services;

public class DesktopAuthenticationStorage : IAuthenticationStorage
{
    public async Task<JsonWebToken> GetJsonWebToken()
    {
        return await Task.Run(() =>
        {
            var jsonWebTokenProperty = Properties.Settings.Default.JsonWebToken;
            var jsonWebToken = JsonConvert.DeserializeObject<JsonWebToken>(jsonWebTokenProperty.ToString());

            return jsonWebToken ?? new JsonWebToken();
        });
    }

    public async Task SetJsonWebToken(JsonWebToken token)
    {
        await Task.Run(() => { 
            Properties.Settings.Default.JsonWebToken = JsonConvert.SerializeObject(token);
            Properties.Settings.Default.Save();
        });
    }

    public async Task DeleteJsonWebToken()
    {
        await Task.Run(() =>
        {
            Properties.Settings.Default.JsonWebToken = string.Empty;
            Properties.Settings.Default.Save();
        });
    }

    public async Task<string> GetAccessToken()
    {
        var jsonWebToken = await GetJsonWebToken();

        return await Task.Run(() => jsonWebToken.AccessToken);
    }

    public async Task<string> GetRefreshToken()
    {
        var jsonWebToken = await GetJsonWebToken();

        return await Task.Run(() => jsonWebToken.RefreshToken);
    }
}
