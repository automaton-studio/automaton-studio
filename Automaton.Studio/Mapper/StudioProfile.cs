using AutoMapper;
using Automaton.Studio.Core;
using Automaton.Studio.Core.Metadata;
using Automaton.Studio.Entities;
using Automaton.Studio.Models;
using Elsa.Models;

namespace Automaton.Studio.Profiles
{
    public class StudioProfile : Profile
    {
        public StudioProfile()
        {
            CreateMap<Runner, WorkflowRunner>();
            CreateMap<ActivityDescriptor, TreeActivity>();
            CreateMap<TreeActivity, StudioActivity>();
            CreateMap<ActivityDefinition, StudioActivity>();
            CreateMap<StudioActivity, ActivityDefinition>();
            CreateMap<StudioFlow, FlowModel>();
            CreateMap<FlowModel, StudioFlow>();
            CreateMap<Flow, StudioFlow>();
            CreateMap<StudioFlow, Flow>();
            CreateMap<Elsa.Models.WorkflowDefinition, WorkflowInfo>();
            CreateMap<Elsa.Models.WorkflowDefinition, StudioWorkflow>();
            CreateMap<StudioWorkflow, Elsa.Models.WorkflowDefinition>();
            CreateMap<StudioConnection, ConnectionDefinition>();
            CreateMap<ConnectionDefinition, StudioConnection>();

        }
    }
}
