using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Sinks.Http;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Automaton.Studio.Logging
{
    public class CustomHttpClient : IHttpClient
    {
        private readonly HttpClient httpClient;

        public CustomHttpClient(IServiceProvider serviceProvider)
        {
            this.httpClient = serviceProvider.GetService<HttpClient>();
        }

        public void Configure(IConfiguration configuration)
        {
        }

        public async Task<HttpResponseMessage> PostAsync(string requestUri, Stream stream)
        {
            using var content = new StreamContent(stream);
            content.Headers.Add("Content-Type", "application/json");

            var response = await httpClient
                .PostAsync(requestUri, content)
                .ConfigureAwait(false);

            return response;
        }

        public void Dispose() => httpClient?.Dispose();
    }
}
