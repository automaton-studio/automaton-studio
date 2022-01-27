using AutoMapper;
using Automaton.Studio.Domain;
using System;
using System.Collections.Generic;

namespace Automaton.Studio.Mapper
{
    public class AutorMapperProfile : Profile
    {
        public AutorMapperProfile()
        {
            CreateMap<Conductor.Flow, Flow>();
            CreateMap<Conductor.Definition, Definition>()
                .ForMember(source => source.Steps, target => target.MapFrom(entity => DeserializeSteps(entity.Steps)));       
        }

        public static IEnumerable<Step> DeserializeSteps(IEnumerable<Conductor.Step> step)
        {
            throw new NotImplementedException();
        }
    }
}
