using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages.Flows
{
    public interface IFlowViewModel
    {
        public ICollection<FlowModel> Flows { get; }

        Task GetFlows();
        Task CreateFlow(string name);
        Task RunFlow(Guid id);
        Task DeleteFlow(Guid id);
    }
}
