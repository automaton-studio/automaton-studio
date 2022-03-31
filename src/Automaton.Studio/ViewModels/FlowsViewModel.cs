using AutoMapper;
using Automaton.Studio.Domain;
using Automaton.Studio.Models;
using Automaton.Studio.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Automaton.Studio.ViewModels
{
    public class FlowsViewModel : IFlowViewModel
    {
        private readonly IMapper mapper;
        private IFlowsService flowsService;
        private IFlowService flowService;

        public ICollection<FlowModel> Flows { get; set;  } = new List<FlowModel>();
      
        public FlowsViewModel
        (
            IFlowsService flowsService,
            IFlowService flowService,
            IMapper mapper
        )
        {
            this.flowsService = flowsService;
            this.flowService = flowService;
            this.mapper = mapper;
        }

        public async Task GetFlows()
        {
            Flows = await flowsService.List();
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
