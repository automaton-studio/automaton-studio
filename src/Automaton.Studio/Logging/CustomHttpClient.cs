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
            httpClient.DefaultRequestHeaders.Add("X-Api-Key", configuration["apiKey"]);
        }

        public async Task<HttpResponseMessage> PostAsync(string requestUri, Stream stream)
        {
            using var content = new StreamContent(stream);
            content.Headers.Add("Content-Type", "application/json");

            // TODO! This is temporary to debug the incoming stream, elete it when ready.
            //StreamReader reader = new StreamReader(stream);
            //string text = reader.ReadToEnd();

            var response = await httpClient
                .PostAsync(requestUri, content)
                .ConfigureAwait(false);

            return response;
        }

        public void Dispose() => httpClient?.Dispose();
    }
}
