using Automaton.Core.Logs;
using Automaton.Runner.Logging;
using Automaton.Runner.Services;
using Microsoft.AspNetCore.Components;
using Serilog;
using System.Net.Http;
using System.Threading.Tasks;

namespace Automaton.Runner
{
    public partial class RunnerApp : ComponentBase
    {
        [Inject] RunnerAppViewModel RunnerAppViewModel { get; set; }
        [Inject] public HttpClient HttpClient { get; set; }
        [Inject] public SerilogHttpClient SerilogHttpClient { get; set; }
        [Inject] public ConfigurationService ConfigurationService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await ConfigureSerilog();
        }

        /// <summary>
        /// Configure Serilog here because we need the injected SerilogHttpClient
        /// and I did not know how to generate it in ServiceCollectionExtension.AddStudio()
        /// </summary>
        /// <returns></returns>
        private async Task ConfigureSerilog()
        {
            await Task.Run(() =>
                Log.Logger = new LoggerConfiguration()
                    .Enrich.FromLogContext()
                    .Enrich.With(new ApplicationEnricher(ConfigurationService))
                    .WriteTo.Http(
                        requestUri: $"{ConfigurationService.BaseUrl}/{ConfigurationService.LogsUrl}",
                        httpClient: new SerilogHttpClient(HttpClient),
                        queueLimitBytes: null)
                    .CreateLogger());

        }
    }
}
