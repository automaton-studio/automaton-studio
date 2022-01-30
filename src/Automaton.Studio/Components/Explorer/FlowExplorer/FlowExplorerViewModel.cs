using AutoMapper;
using Automaton.Studio.Domain;
using System.Collections.Generic;
using System.Linq;

namespace Automaton.Studio.Components.Explorer.FlowExplorer
{
    public class FlowExplorerViewModel : IFlowExplorerViewModel
    {
        private Flow flow;
        private readonly IMapper mapper;

        public IList<FlowExplorerDefinition> ExplorerDefinitions { get; set; }
        public IEnumerable<string> DefinitionNames => ExplorerDefinitions.Select(x => x.Name);

        public FlowExplorerViewModel(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public void Initialize(Flow flow)
        {
            this.flow = flow;
            SetExplorerDefinitions();
            SetStartupDefinition();
        }

        public void RenameDefinition(string definitionId, string workflowName)
        {
            var workflow = ExplorerDefinitions.SingleOrDefault(x => x.Id == definitionId);
            workflow.Name = workflowName;
        }

        public void RefreshDefinitions()
        {
            mapper.Map(flow.Definitions, ExplorerDefinitions);
        }

        public void SetStartupDefinition()
        {
            var startupDefinition = ExplorerDefinitions.SingleOrDefault(x => x.Id == flow.StartupDefinitionId);
            startupDefinition.IsStartup = true;
        }

        private void SetExplorerDefinitions()
        {
            ExplorerDefinitions = mapper.Map<IEnumerable<Definition>, IList<FlowExplorerDefinition>>(flow.Definitions);
        }
    }
}
