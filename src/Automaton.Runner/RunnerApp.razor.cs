using Microsoft.AspNetCore.Components;
using Serilog;
using System.Threading.Tasks;

namespace Automaton.Runner
{
    public partial class RunnerApp : ComponentBase
    {
        [Inject] RunnerAppViewModel RunnerAppViewModel { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await ConfigureSerilog();
        }

        /// <summary>
        /// Configure Serilog here because we need the injected CustomHttpClient
        /// and I did not know how to generate it in ServiceCollectionExtension.AddStudio()
        /// </summary>
        /// <returns></returns>
        private async Task ConfigureSerilog()
        {
            await Task.Run(() =>
                Log.Logger = new LoggerConfiguration()
                    .Enrich.FromLogContext()
                    .CreateLogger());
        }
    }
}
