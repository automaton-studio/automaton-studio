using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Automaton.Runner.Core.Auth
{
    public class AuthService : IAuthService
    {
        public async Task<JsonWebToken> GetToken(UserCredentials userCredentials)
        {
            var userDetailsAsJson = JsonConvert.SerializeObject(userCredentials);
            var userDetailsContent = new StringContent(userDetailsAsJson, Encoding.UTF8, "application/json");

            using var httpClient = new HttpClient();
            var response = await httpClient.PostAsync("https://localhost:5001/api/token", userDetailsContent);

            var authTokenJson = await response.Content.ReadAsStringAsync();
            var authToken = JsonConvert.DeserializeObject<JsonWebToken>(authTokenJson);

            return authToken;
        }
    }
}
