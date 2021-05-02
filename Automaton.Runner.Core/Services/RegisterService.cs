using Automaton.Runner.Core.Services;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Automaton.Runner.Services
{
    public class RegisterService : IRegistrationService
    {
        private readonly ConfigService configService;
        private readonly IAuthService authService;

        public RegisterService(IAuthService authService, ConfigService configService)
        {
            this.authService = authService;
            this.configService = configService;
        }

        public async Task Register(string runnerName)
        {
            var runnerNameJson = JsonConvert.SerializeObject(new { RunnerName = runnerName });
            var runnerNameContent = new StringContent(runnerNameJson, Encoding.UTF8, "application/json");

            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authService.Token.AccessToken);
            var response = await httpClient.PostAsync(configService.StudioConfig.RegistrationApiUrl, runnerNameContent);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                var returnObject = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException(returnObject);
            }
        }
    }
}
