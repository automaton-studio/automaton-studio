using Elsa.Models;
using System.Threading.Tasks;

namespace Automaton.Studio.Services
{
    public interface IWorkflowService
    {
        Task RunWorkflow(string workflowId);
        Task RunWorkflow(WorkflowDefinition workflowDefinition);
    }
}
