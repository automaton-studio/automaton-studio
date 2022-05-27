using Automaton.Client.Auth.Services;

namespace Automaton.Client.Auth.Http
{
    public class AutomatonHttpClient
    {
        private readonly ConfigurationService configService;

        public HttpClient Client { get; }

        public AutomatonHttpClient(HttpClient httpClient, ConfigurationService configService)
        {
            this.configService = configService;
            this.Client = httpClient;
            this.Client.BaseAddress = new Uri(configService.BaseUrl);
            this.Client.Timeout = new TimeSpan(0, 0, 30);
            this.Client.DefaultRequestHeaders.Clear();
        }
    }
}
