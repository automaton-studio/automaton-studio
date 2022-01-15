using Automaton.Studio.Conductor;
using System.Threading.Tasks;

namespace Automaton.Studio.Services.Interfaces
{
    public interface ISolutionService
    {
        Task<Flow> Load(string filePath);
        Task Save(Flow flow);
        Task SaveAs(Flow flow, string filePath);
    }
}
