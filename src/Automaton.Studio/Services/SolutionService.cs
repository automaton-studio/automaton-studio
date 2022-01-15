using Automaton.Studio.Conductor;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Automaton.Studio.Services.Interfaces
{
    public class SolutionService : ISolutionService
    {
        public async Task<Flow> Load(string filePath)
        {
            using var openStream = File.OpenRead(filePath);
            var flow = await JsonSerializer.DeserializeAsync<Flow>(openStream);
            return flow;
        }

        public async Task Save(Flow flow)
        {
            await SaveAs(flow, flow.SavedFilePath);
        }

        public async Task SaveAs(Flow flow, string filePath)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            using var createStream = File.Create(filePath);
            await JsonSerializer.SerializeAsync(createStream, flow, options);
            await createStream.DisposeAsync();
        }
    }
}
