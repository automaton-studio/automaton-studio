using Automaton.Runner.Core.Services;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Automaton.Runner.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly AppConfigurationService configService;
        private readonly IAuthService authService;

        public RegistrationService(IAuthService authService, AppConfigurationService configService)
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

            response.EnsureSuccessStatusCode();
        }
    }
}
