using AutoMapper;
using Automaton.Studio.Attributes;
using Automaton.Studio.Domain;
using Automaton.Studio.Domain.Interfaces;
using Automaton.Studio.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Automaton.Studio.Steps.ExecuteFlow
{
    [StepDescription(
        Name = "ExecuteFlow",
        Type = "ExecuteFlow",
        DisplayName = "Execute Flow",
        Category = "Console",
        Description = "Execute flow",
        Icon = "code"
    )]
    public class ExecuteFlowStep : StudioStep
    {
        private readonly IMapper mapper;
        private readonly FlowsService flowsService;

        public IEnumerable<FlowModel> Flows { get; set; } = new List<FlowModel>();

        public string FlowName
        {
            get
            {
                var flow = Flows.SingleOrDefault(x => x.Id == FlowId);
                return flow != null ? flow.Name : string.Empty;
            }
        }

        public Guid FlowId
        {
            get
            {
                if (Inputs.ContainsKey(nameof(FlowId)))
                {
                    var guid = Inputs[nameof(FlowId)].ToString();
                    Guid.TryParse(guid, out Guid flowId);
                    return flowId;
                }

                return Guid.Empty;
            }
            set
            {
                Inputs[nameof(FlowId)] = value;
            }
        }

        public ExecuteFlowStep(IMapper mapper, FlowsService flowsService)
        {
            this.flowsService = flowsService;
            this.mapper = mapper;

            var flowsInfo = Task.Run(async () => await this.flowsService.List()).Result;
            Flows = this.mapper.Map<ICollection<FlowModel>>(flowsInfo);
        }

        public override Type GetDesignerComponent()
        {
            return typeof(ExecuteFlowDesigner);
        }

        public override Type GetPropertiesComponent()
        {
            return typeof(ExecuteFlowProperties);
        }
    }
}
