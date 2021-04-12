using System.Threading.Tasks;

namespace Automaton.Runner.Core
{
    public interface IWorkflowService
    {
        public Task RunWorkflow(string workflowId);
    }
}
