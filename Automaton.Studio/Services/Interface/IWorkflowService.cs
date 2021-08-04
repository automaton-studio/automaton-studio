using Automaton.Studio.Core;
using Elsa.Models;
using System.Threading.Tasks;

namespace Automaton.Studio.Services
{
    public interface IWorkflowService
    {
        Task<StudioWorkflow> LoadWorkflow(string workflowId);
        Task SaveWorkflow(StudioWorkflow studioWorkflow);
        Task RunWorkflow(string workflowId);
        Task RunWorkflow(StudioWorkflow studioWorkflow);
    }
}
