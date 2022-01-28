using AutoMapper;
using Automaton.Studio.Domain;
using Automaton.Studio.Factories;
using System.Collections.Generic;

namespace Automaton.Studio.Config
{
    public class AutorMapperProfile : Profile
    {
        private readonly StepFactory stepFactory = new StepFactory(new StepTypeDescriptor());

        public AutorMapperProfile()
        {
            CreateMap<Conductor.Step, Step>();
            CreateMap<Conductor.Flow, Flow>();
            CreateMap<Conductor.Definition, Definition>()
                .ForMember(source => source.Steps, target => target.MapFrom(entity => DeserializeSteps(entity.Steps)));       
        }

        public IEnumerable<Step> DeserializeSteps(IEnumerable<Conductor.Step> conductorSteps)
        {
            // I know this is a bit unusual, but we can't use the static Mapper version here.
            // Because we really need a maper instance, we create it like below.
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutorMapperProfile());
            });
            var mapper = mappingConfig.CreateMapper();
         
            foreach (var conductorStep in conductorSteps)
            {
                // Create domain step
                var step = stepFactory.CreateStep(conductorStep.Name);

                // Update step properties using mapper
                mapper.Map(conductorStep, step);

                yield return step;
            }
        }
    }
}
