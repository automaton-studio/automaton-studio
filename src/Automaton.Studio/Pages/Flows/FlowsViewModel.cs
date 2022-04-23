using AutoMapper;
using Automaton.Studio.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages.Flows
{
    public class FlowsViewModel
    {
        private readonly IMapper mapper;
        private readonly FlowsService flowsService;
        private readonly FlowService flowService;

        public ICollection<FlowModel> Flows { get; set;  } = new List<FlowModel>();
      
        public FlowsViewModel
        (
            FlowsService flowsService,
            FlowService flowService,
            IMapper mapper
        )
        {
            this.flowsService = flowsService;
            this.flowService = flowService;
            this.mapper = mapper;
        }

        public async Task GetFlows()
        {
            var flows = await flowsService.List();

            Flows = mapper.Map<ICollection<FlowModel>>(flows);
        }

        public async Task CreateFlow(string name)
        {
            var flow = await flowService.Create(name);

            var flowModel = new FlowModel
            {
                Id = flow.Id,
                Name = flow.Name
            };

            Flows.Add(flowModel);
        }

        public async Task DeleteFlow(Guid id)
        {
            await flowService.Delete(id);

            var flow = Flows.SingleOrDefault(x => x.Id == id);

            Flows.Remove(flow);
        }

        public async Task RunFlow(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
