using AutoMapper;
using Automaton.Studio.Components.Explorer.FlowExplorer;
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
            CreateMap<Step, Dto.Step>();
            CreateMap<Flow, Dto.Flow>();
            CreateMap<Definition, Dto.Definition>();
            
            CreateMap<Dto.Step, Step>();
            CreateMap<Dto.Flow, Flow>();
            CreateMap<Dto.Definition, Definition>()
                .ForMember
                (
                    source => source.Steps, 
                    target => target.MapFrom(entity => DeserializeSteps(entity.Steps))
                );

            CreateMap<Definition, FlowExplorerDefinition>();           
        }

        public IEnumerable<Step> DeserializeSteps(IEnumerable<Dto.Step> conductorSteps)
        {
            foreach (var conductorStep in conductorSteps)
            {
                // Use Conductor step name to create Domain step
                var step = stepFactory.CreateStep(conductorStep.Name);

                // Deserialized steps are marked as final 
                step.MarkAsFinal();

                // Update step properties using AutoMapper
                var mapper = GetMapperInstance();
                mapper.Map(conductorStep, step);

                yield return step;
            }
        }

        /// <summary>
        /// This is a bit unusual, but we can't use the static Mapper version in this class.
        /// Because we really need a maper instance, we create it like below.
        /// </summary>
        /// <returns>IMapper instance</returns>
        private static IMapper GetMapperInstance()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutorMapperProfile());
            });
            var mapper = mappingConfig.CreateMapper();

            return mapper;
        }
    }
}
