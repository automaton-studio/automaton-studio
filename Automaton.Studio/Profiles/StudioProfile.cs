using AutoMapper;
using Automaton.Studio.Activities;
using Automaton.Studio.Models;
using Elsa.Metadata;
using Elsa.Models;

namespace Automaton.Studio.Profiles
{
    public class StudioProfile : Profile
    {
        public StudioProfile()
        {
            CreateMap<Runner, RunnerModel>();
            CreateMap<WorkflowDefinition, WorkflowModel>();
            CreateMap<ActivityDescriptor, ActivityTreeModel>();

            // Activities
            CreateMap<ActivityDefinition, WriteLineActivity>();
        }
    }
}
