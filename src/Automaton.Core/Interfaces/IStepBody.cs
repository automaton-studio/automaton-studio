using Automaton.Core.Models;
using System.Threading.Tasks;
using WorkflowCore.Models;

namespace Automaton.Core.Interfaces
{
    public interface IStepBody
    {        
        Task<ExecutionResult> RunAsync(IStepExecutionContext context);        
    }
}
