using AutoMapper;
using Automaton.Studio.Models;
using Elsa.Client.Models;

namespace Automaton.Studio.Profiles
{
    public class StudioProfile : Profile
    {
        public StudioProfile()
        {
            CreateMap<Runner, RunnerModel>();
            CreateMap<WorkflowDefinition, WorkflowModel>();
            CreateMap<Elsa.Metadata.ActivityDescriptor, ActivityTreeItem>();
        }
    }
}
