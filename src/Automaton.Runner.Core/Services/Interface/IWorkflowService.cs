using System.Threading.Tasks;

namespace Automaton.Runner.Services
{
    public interface IWorkflowService
    {
        public Task RunWorkflow(string workflowId);
    }
}
