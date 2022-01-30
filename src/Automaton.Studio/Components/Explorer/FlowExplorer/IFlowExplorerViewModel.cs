using Automaton.Studio.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Automaton.Studio.Components.Explorer.FlowExplorer
{
    public interface IFlowExplorerViewModel
    {
        IList<FlowExplorerDefinition> Definitions { get; set; }
        IEnumerable<string> DefinitionNames { get; }

        Task LoadFlow(string flowId);
        void RenameWorkflow(string workflowId, string workflowName);
        void AddWorkflow(FlowExplorerDefinition workflowModel);

    }
}
