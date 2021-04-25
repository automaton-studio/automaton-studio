using Automaton.Runner.Core;
using Automaton.Runner.Core.Services;
using Newtonsoft.Json;
using System.Net.Http;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace Automaton.Runner.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppConfigurationService configService;

        public JsonWebToken Token { get; set; }

        public AuthService(AppConfigurationService configService)
        {
            this.configService = configService;
        }

        public async Task SignIn(string username, string password)
        {
            var userDetailsAsJson = JsonConvert.SerializeObject(new { UserName = username, Password = password });
            var userDetailsContent = new StringContent(userDetailsAsJson, Encoding.UTF8, "application/json");

            using var httpClient = new HttpClient();
            var response = await httpClient.PostAsync(configService.StudioConfig.TokenApiUrl, userDetailsContent);

            if (!response.IsSuccessStatusCode)
            {
                throw new AuthenticationException();
            }

            Token = JsonConvert.DeserializeObject<JsonWebToken>(await response.Content.ReadAsStringAsync());
        }
    }
}
