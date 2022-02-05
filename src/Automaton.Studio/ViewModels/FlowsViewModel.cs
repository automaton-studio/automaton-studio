using AutoMapper;
using Automaton.Studio.Domain;
using Automaton.Studio.Models;
using Automaton.Studio.Services.Interfaces;
using System;
using System.Collections.Generic;
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
            var flow = new Flow { Name = name };
            await flowService.Create(flow);

            AddFlowModel(flow);
        }

        public async Task DeleteFlow(FlowModel flow)
        {
            await flowService.Delete(flow.Id);

            Flows.Remove(flow);
        }

        public async Task RunFlow(FlowModel flow)
        {
            throw new NotImplementedException();
        }

        private void AddFlowModel(Flow flow)
        {
            var flowModel = new FlowModel 
            { 
                Id = flow.Id, 
                Name = flow.Name 
            };

            Flows.Add(flowModel);
        }
    }
}
