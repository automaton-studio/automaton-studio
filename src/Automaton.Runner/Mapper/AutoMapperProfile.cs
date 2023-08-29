using AutoMapper;
using Automaton.Core.Models;
using Automaton.Runner.Models;

namespace Automaton.Runner.Mapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<WorkflowExecution, FlowExecution>();
    }
}
