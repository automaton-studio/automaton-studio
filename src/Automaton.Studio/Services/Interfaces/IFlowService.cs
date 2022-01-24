using Automaton.Studio.Conductor;
using Automaton.Studio.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Automaton.Studio.Services.Interfaces
{
    public interface IFlowService
    {
        Task<IEnumerable<FlowModel>> List();
        Task<Flow> Load(string flowId);
        Task Save(Flow flow);
    }
}
