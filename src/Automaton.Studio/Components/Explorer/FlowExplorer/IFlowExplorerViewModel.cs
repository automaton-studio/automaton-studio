using Automaton.Studio.Domain;
using System.Collections.Generic;

namespace Automaton.Studio.Components.Explorer.FlowExplorer
{
    public interface IFlowExplorerViewModel
    {
        IEnumerable<string> DefinitionNames { get; }
        IList<FlowExplorerDefinition> ExplorerDefinitions { get; set; }

        void LoadDefinitions(Flow flow);
        void RenameDefinition(string workflowId, string workflowName);
        void SetStartupDefinition(string definitionId);
        void RefreshDefinitions();
        void DeleteDefinition(FlowExplorerDefinition definition);
    }
}
