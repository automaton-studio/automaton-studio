using AutoMapper;
using Automaton.Studio.Models;
using Automaton.Studio.Activities;
using Elsa.Models;
using Automaton.Studio.Activity.Metadata;

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

            // Activities
            CreateMap<ActivityDefinition, WriteLineActivity>();
        }
    }
}
