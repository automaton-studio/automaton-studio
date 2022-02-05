using Automaton.Studio.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Automaton.Studio.ViewModels
{
    public interface IFlowViewModel
    {
        public ICollection<FlowModel> Flows { get; }

        Task GetFlows();
        Task CreateFlow(string name);
        Task RunFlow(FlowModel flow);
        Task DeleteFlow(FlowModel flow);
    }
}
