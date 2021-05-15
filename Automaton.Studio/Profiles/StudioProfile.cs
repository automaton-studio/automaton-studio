using AutoMapper;
using Automaton.Studio.Models;
using Automaton.Activity.WriteLine;
using Elsa.Metadata;
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

            // Activities
            CreateMap<ActivityDefinition, WriteLineActivity>();
        }
    }
}
