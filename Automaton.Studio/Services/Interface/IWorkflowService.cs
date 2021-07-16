using System.Threading.Tasks;

namespace Automaton.Studio.Services
{
    public interface IWorkflowService
    {
        public Task RunWorkflow(string workflowId);
    }
}
