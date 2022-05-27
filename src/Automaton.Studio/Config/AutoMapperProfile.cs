using AutoMapper;
using Automaton.Client.Auth.Models;
using Automaton.Core.Dto;
using Automaton.Studio.Domain;
using Automaton.Studio.Factories;
using Automaton.Studio.Pages.Designer.Components.FlowExplorer;
using Automaton.Studio.Pages.Flows;
using Automaton.Studio.Pages.Login;
using System.Collections.Generic;

namespace Automaton.Studio.Config
{
    public class AutoMapperProfile : Profile
    {
        private readonly StepFactory stepFactory = new StepFactory(new StepTypeDescriptor());

        public AutoMapperProfile()
        {
            CreateMap<Step, StepDto>();
            CreateMap<Flow, FlowDto>();
            CreateMap<Definition, DefinitionDto>();
            
            CreateMap<StepDto, Step>();
            CreateMap<FlowDto, Flow>().AfterMap((source, target) => FlowCreated(source, target));

            CreateMap<DefinitionDto, Definition>().ForMember
            (
                source => source.Steps, 
                target => target.MapFrom(entity => SetupSteps(entity.Steps))
            )
            .AfterMap((source, target) => DefinitionCreated(source, target));

            CreateMap<Definition, FlowExplorerDefinition>();
            CreateMap<LoginModel, LoginDetails>();
        }

        private static void FlowCreated(FlowDto source, Flow target)
        {
            foreach(var definition in target.Definitions)
            {
                definition.Flow = target;
            }
        }

        private static void DefinitionCreated(DefinitionDto source, Definition target)
        {
            foreach (var step in target.Steps)
            {
                step.Definition = target;
            }
        }

        public IEnumerable<Step> SetupSteps(IEnumerable<StepDto> stepDtos)
        {
            foreach (var stepDto in stepDtos)
            {
                // Use Conductor step name to create Domain step
                var step = stepFactory.CreateStep(stepDto.Name);

                // Deserialized steps are marked as final 
                step.MarkAsFinal();

                // Update step properties using AutoMapper
                var mapper = GetMapperInstance();
                mapper.Map(stepDto, step);

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
                mc.AddProfile(new AutoMapperProfile());
            });
            var mapper = mappingConfig.CreateMapper();

            return mapper;
        }
    }
}
