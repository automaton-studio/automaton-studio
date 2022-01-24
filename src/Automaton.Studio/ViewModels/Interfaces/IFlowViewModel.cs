using Automaton.Studio.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Automaton.Studio.ViewModels
{
    public interface IFlowViewModel
    {
        Task<IEnumerable<FlowModel>> GetFlows();
        Task<FlowModel> CreateFlow(string name);
        Task RunFlow(FlowModel flow);
        void DeleteFlow(string id);
    }
}
