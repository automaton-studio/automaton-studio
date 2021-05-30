using AutoMapper;
using Automaton.Studio.Activity;
using Automaton.Studio.Metadata;
using Automaton.Studio.Models;
using Elsa.Models;

namespace Automaton.Studio.Profiles
{
    public class StudioProfile : Profile
    {
        public StudioProfile()
        {
            CreateMap<Runner, RunnerModel>();
            CreateMap<ActivityDescriptor, TreeActivity>();
            CreateMap<TreeActivity, StudioActivity>();
            CreateMap<NewWorkflowModel, WorkflowDefinition>();
            CreateMap<ActivityDefinition, StudioActivity>();
            CreateMap<WorkflowDefinition, StudioWorkflow>()
                .ForMember(d => d.Activities, option => option.Ignore()); ;
        }
    }
}
