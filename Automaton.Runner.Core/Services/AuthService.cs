using Automaton.Runner.Core;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Automaton.Runner.Services
{
    public class AuthService : IAuthService
    {
        public async Task<JsonWebToken> GetToken(UserCredentials userCredentials, string tokenApiUrl)
        {
            var userDetailsAsJson = JsonConvert.SerializeObject(userCredentials);
            var userDetailsContent = new StringContent(userDetailsAsJson, Encoding.UTF8, "application/json");

            using var httpClient = new HttpClient();
            var response = await httpClient.PostAsync(tokenApiUrl, userDetailsContent);

            var authTokenJson = await response.Content.ReadAsStringAsync();
            var authToken = JsonConvert.DeserializeObject<JsonWebToken>(authTokenJson);

            return authToken;
        }
    }
}
