using Automaton.Runner.Core;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace Automaton.Runner.Services
{
    public class AuthService : IAuthService
    {
        public JsonWebToken Token { get; set; }

        public async Task SignIn(string username, string password, string tokenApiUrl)
        {
            var userDetailsAsJson = JsonConvert.SerializeObject(new { UserName = username, Password = password });
            var userDetailsContent = new StringContent(userDetailsAsJson, Encoding.UTF8, "application/json");

            using var httpClient = new HttpClient();
            var response = await httpClient.PostAsync(tokenApiUrl, userDetailsContent);

            if (!response.IsSuccessStatusCode)
            {
                throw new AuthenticationException();
            }

            Token = JsonConvert.DeserializeObject<JsonWebToken>(await response.Content.ReadAsStringAsync());
        }
    }
}
