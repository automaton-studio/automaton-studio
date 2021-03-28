using System.Threading.Tasks;

namespace Automaton.Runner.Core
{
    public interface IWorkflowManager
    {
        public Task RunWorkflow(string workflowId);
    }
}
