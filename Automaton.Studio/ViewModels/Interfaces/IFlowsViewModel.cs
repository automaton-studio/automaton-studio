using Automaton.Studio.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Automaton.Studio.ViewModels
{
    public interface IFlowsViewModel
    {
        IList<FlowModel> Flows { get; set; }
        IEnumerable<WorkflowRunner> Runners { get; set; }
        FlowModel NewFlow { get; set; }

        void Initialize();
        Task<FlowModel> CreateNewFlow();
        Task RunWorkflow(FlowModel workflow);
        void DeleteFlow(FlowModel workflow);
    }
}
