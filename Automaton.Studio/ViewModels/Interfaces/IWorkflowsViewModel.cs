using Automaton.Studio.Models;
using Elsa.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Automaton.Studio.ViewModels
{
    public interface IWorkflowsViewModel
    {
        IEnumerable<WorkflowInfo> Workflows { get; set; }
        IEnumerable<WorkflowRunner> Runners { get; set; }
        WorkflowNew NewWorkflowDetails { get; set; }

        Task Initialize();
        Task<WorkflowDefinition> NewWorkflow();
        Task RunWorkflow(WorkflowInfo workflow);
    }
}
