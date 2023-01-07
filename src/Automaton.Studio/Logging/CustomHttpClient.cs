using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
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
            // Does nothing for now
        }

        public async Task<HttpResponseMessage> PostAsync(string requestUri, Stream stream)
        {
            var reader = new StreamReader(stream);
            var text = reader.ReadToEnd();
            var logEvents = JsonConvert.DeserializeObject<IEnumerable<LogEvent>>(text);
               
            var response = await httpClient.PostAsJsonAsync(requestUri, logEvents);

            return response;
        }

        public void Dispose()
        {
            httpClient?.Dispose();
        }
    }
}
