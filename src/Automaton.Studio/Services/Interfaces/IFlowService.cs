using Automaton.Studio.Conductor;
using System.Threading.Tasks;

namespace Automaton.Studio.Services.Interfaces
{
    public interface IFlowService
    {
        Task<Flow> Load(string flowId);
        Task Save(Flow flow);
    }
}
