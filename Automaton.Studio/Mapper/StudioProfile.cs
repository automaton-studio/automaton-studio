using AutoMapper;
using Automaton.Studio.Core;
using Automaton.Studio.Core.Metadata;
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
            CreateMap<FlowModel, WorkflowDefinition>();
            CreateMap<ActivityDefinition, StudioActivity>();
            CreateMap<StudioActivity, ActivityDefinition>();
            CreateMap<Entities.Flow, FlowModel>();
            CreateMap<FlowModel, Entities.Flow>();
            CreateMap<WorkflowDefinition, WorkflowInfo>();
            CreateMap<WorkflowDefinition, StudioWorkflow>();
            CreateMap<StudioWorkflow, WorkflowDefinition>();
            CreateMap<StudioConnection, ConnectionDefinition>();
            CreateMap<ConnectionDefinition, StudioConnection>();

        }
    }
}
