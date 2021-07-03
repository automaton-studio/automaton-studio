using Automaton.Studio.Models;
using Elsa.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Automaton.Studio.ViewModels
{
    public interface IWorkflowsViewModel
    {
        IList<WorkflowInfo> Workflows { get; set; }
        IEnumerable<WorkflowRunner> Runners { get; set; }
        NewWorkflow NewWorkflowDetails { get; set; }

        Task Initialize();
        Task<WorkflowDefinition> NewWorkflow();
        Task RunWorkflow(WorkflowInfo workflow);
        Task DeleteWorkflow(WorkflowInfo workflow);
    }
}
