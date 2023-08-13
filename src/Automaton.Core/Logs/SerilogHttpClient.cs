using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Serilog.Sinks.Http;
using System.Net.Http.Json;

namespace Automaton.Core.Logs
{
    public class SerilogHttpClient : IHttpClient
    {
        private readonly HttpClient httpClient;

        public SerilogHttpClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        ~SerilogHttpClient()
        {
            Dispose(false);
        }

        public void Configure(IConfiguration configuration)
        {
        }

        public async Task<HttpResponseMessage> PostAsync(string requestUri, Stream stream)
        {
            var reader = new StreamReader(stream);
            var logEventsText = reader.ReadToEnd();
            var logEvents = JsonConvert.DeserializeObject<IEnumerable<SerilogHttpLogEvent>>(logEventsText);

            var response = await httpClient.PostAsJsonAsync(requestUri, logEvents);

            return response;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                httpClient.Dispose();
            }
        }
    }
}
