using AutoMapper;
using Automaton.Studio.Domain;
using Automaton.Studio.Models;
using Automaton.Studio.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Automaton.Studio.Components.Explorer.FlowExplorer
{
    public class FlowExplorerViewModel : IFlowExplorerViewModel
    {
        private readonly IMapper mapper;
        private readonly IFlowService flowService;

        public IList<FlowExplorerDefinition> Definitions { get; set; }
        public IEnumerable<string> DefinitionNames => Definitions.Select(x => x.Name);

        public FlowExplorerViewModel(IFlowService flowService, IMapper mapper)
        {
            this.mapper = mapper;
            this.flowService = flowService;
        }

        public async Task LoadFlow(string flowId)
        {
            var flow = await flowService.Load(flowId);
            Definitions = mapper.Map<IEnumerable<Definition>, IList<FlowExplorerDefinition>>(flow.Definitions);
        }

        public void RenameWorkflow(string workflowId, string workflowName)
        {
            var workflow = Definitions.SingleOrDefault(x => x.Id == workflowId);
            workflow.Name = workflowName;
        }

        public void AddWorkflow(FlowExplorerDefinition workflowModel)
        {
            Definitions.Add(workflowModel);
        }
    }
}
