using Automaton.Studio.Domain;
using System.Collections.Generic;

namespace Automaton.Studio.Components.Explorer.FlowExplorer
{
    public interface IFlowExplorerViewModel
    {
        IEnumerable<string> DefinitionNames { get; }
        IList<FlowExplorerDefinition> ExplorerDefinitions { get; set; }

        void Initialize(Flow flow);
        void RenameDefinition(string workflowId, string workflowName);
        void SetStartupDefinition();
        void RefreshDefinitions();
    }
}
