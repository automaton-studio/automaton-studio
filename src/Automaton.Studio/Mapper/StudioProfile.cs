using AutoMapper;
using Automaton.Studio.Conductor;
using Automaton.Studio.Models;

namespace Automaton.Studio.Mapper
{
    public class StudioProfile : Profile
    {
        public StudioProfile()
        {
            CreateMap<Definition, DefinitionModel>();
        }
    }
}
