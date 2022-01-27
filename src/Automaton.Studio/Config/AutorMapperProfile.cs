using AutoMapper;
using Automaton.Studio.Domain;
using Automaton.Studio.Models;

namespace Automaton.Studio.Mapper
{
    public class AutorMapperProfile : Profile
    {
        public AutorMapperProfile()
        {
            CreateMap<Definition, DefinitionModel>();
        }
    }
}
