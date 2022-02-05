using Automaton.Studio.Domain;
using Automaton.Studio.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Automaton.Studio.Services.Interfaces
{
    public interface IFlowService
    {
        Task<IEnumerable<FlowModel>> List();
        Task<Flow> Load(string flowId);
        Task Create(Flow flow);
        Task Update(Flow flow);
        Task Delete (string flowId);
    }
}
