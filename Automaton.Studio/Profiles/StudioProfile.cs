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
            CreateMap<ActivityDescriptor, TreeActivityModel>();
            CreateMap<WorkflowDefinition, WorkflowModel>();
            CreateMap<NewWorkflowModel, WorkflowDefinition>();
            CreateMap<ActivityDefinition, StudioActivity>();
        }
    }
}
