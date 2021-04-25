using System.Collections.Generic;
using System.Threading.Tasks;

namespace Automaton.Studio.ViewModels
{
    public interface IWorkflowsViewModel
    {
        ICollection<Elsa.Client.Models.WorkflowDefinition> Workflows { get; set; }
        Task LoadWorkflows();
        Task RunWorkflow(string workflowId, string connectionId);     
    }
}
