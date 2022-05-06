using Automaton.Runner.Core.Services;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Automaton.Runner.Services
{
    public class RegisterService
    {
        private readonly ConfigService configService;
        private readonly HttpClient httpClient;

        public RegisterService(HttpClient httpClient, ConfigService configService)
        {
            this.httpClient = httpClient;
            this.configService = configService;
        }

        public async Task Register(string runnerName)
        {
            var runnerNameJson = JsonConvert.SerializeObject(new { Name = runnerName });
            var runnerNameContent = new StringContent(runnerNameJson, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(configService.ApiConfig.RegistrationApiUrl, runnerNameContent);

            response.EnsureSuccessStatusCode();
        }
    }
}
