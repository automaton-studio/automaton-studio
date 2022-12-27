using Automaton.Client.Auth.Interfaces;
using Automaton.Studio.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Sinks.Http;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Automaton.Studio.Logging
{
    public class CustomHttpClient : IHttpClient
    {
        private const string Bearer = "bearer";
        private readonly IServiceProvider serviceProvider;
        private readonly HttpClient httpClient;

        public CustomHttpClient(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.httpClient = serviceProvider.GetService<HttpClient>();
        }

        public void Configure(IConfiguration configuration)
        {
            httpClient.DefaultRequestHeaders.Add("X-Api-Key", configuration["apiKey"]);
        }

        public async Task<HttpResponseMessage> PostAsync(string requestUri, Stream stream)
        {
            try
            {
                using var content = new StreamContent(stream);
                content.Headers.Add("Content-Type", "application/json");

                //StreamReader reader = new StreamReader(stream);
                //string text = reader.ReadToEnd();

                var response = await httpClient
                    .PostAsync(requestUri, content)
                    .ConfigureAwait(false);

                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
           
        }

        public void Dispose() => httpClient?.Dispose();
    }
}
