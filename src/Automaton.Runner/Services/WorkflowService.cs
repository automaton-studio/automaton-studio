using Automaton.Core.Services;
using System;
using System.Threading.Tasks;

namespace Automaton.Runner.Services
{
    public class WorkflowService
    {
        private readonly WorkflowExecutor workflowExecutor;

        public WorkflowService(WorkflowExecutor workflowDefinitionStore)
        {
            this.workflowExecutor = workflowDefinitionStore;
        }

        public async Task RunWorkflow(Guid workflowId)
        {
            await workflowExecutor.Execute(workflowId);
        }
    }
}
