﻿using AutoMapper;
using Automaton.Core.Interfaces;
using Automaton.Core.Models;
using Automaton.WebApi.Models;
using System.Dynamic;
using System.Linq.Expressions;

namespace Automaton.Studio.Config
{
    public class AutorMapperProfile : Profile
    {
        private IServiceCollection serviceCollection;
        private IServiceProvider serviceProvider;

        public AutorMapperProfile()
        {
            CreateServices();
            CreateMaps();
        }

        private void CreateServices()
        {
            serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging();
            serviceCollection.AddSteps();
            serviceProvider = serviceCollection.BuildServiceProvider();
        }

        private void CreateMaps()
        {
           
        }

        public IEnumerable<WorkflowStep> ConvertSteps(IEnumerable<Step> steps)
        {
            foreach (var step in steps)
            {
                var type = FindType(step.Name);

                var targetStep = serviceProvider.GetService(type) as WorkflowStep;

                // Update step properties using AutoMapper
                var mapper = GetMapperInstance();
                mapper.Map(step, targetStep);
               
                yield return targetStep;
            }
        }

        private static Type FindType(string name)
        {
            return Type.GetType($"Automaton.Steps.{name}, Automaton.Steps", true, true);
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