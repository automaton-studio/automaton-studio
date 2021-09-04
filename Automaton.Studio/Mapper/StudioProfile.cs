using AutoMapper;
using Automaton.Studio.Core;
using Automaton.Studio.Core.Metadata;
using Automaton.Studio.Entities;
using Automaton.Studio.Models;

namespace Automaton.Studio.Profiles
{
    public class StudioProfile : Profile
    {
        public StudioProfile()
        {
            // Automaton mapping
            CreateMap<Runner, RunnerModel>();
            CreateMap<ActivityDescriptor, ActivityModel>();
            CreateMap<ActivityModel, StudioActivity>();
            CreateMap<StudioFlow, FlowModel>();
            CreateMap<FlowModel, StudioFlow>();
            CreateMap<Flow, FlowModel>();
            CreateMap<FlowModel, Flow>();
            CreateMap<Flow, StudioFlow>();
            CreateMap<StudioFlow, Flow>();
            CreateMap<StudioWorkflow, WorkflowDefinition>();
            CreateMap<WorkflowDefinition, StudioWorkflow>();
            CreateMap<StudioWorkflow, WorkflowModel>();

            // Elsa mapping
            CreateMap<Elsa.Models.WorkflowDefinition, StudioWorkflow>();
            CreateMap<StudioWorkflow, Elsa.Models.WorkflowDefinition>();
            CreateMap<Elsa.Models.ActivityDefinition, StudioActivity>();
            CreateMap<StudioActivity, Elsa.Models.ActivityDefinition>();
            CreateMap<StudioConnection, Elsa.Models.ConnectionDefinition>();
            CreateMap<Elsa.Models.ConnectionDefinition, StudioConnection>();
        }
    }
}
