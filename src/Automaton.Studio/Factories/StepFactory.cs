using Automaton.Studio.Domain;
using Automaton.Studio.Domain.Interfaces;
using Automaton.Studio.Extensions;
using Automaton.Studio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Automaton.Studio.Factories
{
    public class StepFactory
    {
        private const string StepsAssembly = "Automaton.Studio";

        private IDictionary<string, StepExplorerModel> solutionSteps;
        private IDictionary<string, Type> solutionTypes;
        private IStepTypeDescriptor stepTypeDescriptor;

        public StepFactory(IStepTypeDescriptor stepTypeDescriber)
        {
            this.stepTypeDescriptor = stepTypeDescriber;
            solutionSteps = new Dictionary<string, StepExplorerModel>();
            solutionTypes = new Dictionary<string, Type>();

            var assembly = Assembly.Load(StepsAssembly);
            AddActivitiesFrom(assembly);
        }

        public IEnumerable<StepExplorerModel> GetSteps()
        {
            return solutionSteps.Values;
        }

        public void AddActivitiesFrom(Assembly assembly)
        {
            AddActivitiesFrom(new[] { assembly });
        }

        public void AddActivitiesFrom(IEnumerable<Assembly> assemblies)
        {
            var types = assemblies.SelectMany(x => x.GetAllWithBaseClass<Step>()).Where(x => !x.IsAbstract);

            foreach (var type in types)
            {
                AddStep(type);
            }
        }

        public void AddStep(Type stepType)
        {
            var stepDescriptor = stepTypeDescriptor.Describe(stepType);

            var solutionStep = new StepExplorerModel 
            { 
                Name = stepDescriptor.Name, 
                Type = stepDescriptor.Type,
                Description = stepDescriptor.Description,
                DisplayName = stepDescriptor.DisplayName,
                Category = stepDescriptor.Category,
                Icon = stepDescriptor.Icon
            };

            solutionSteps.Add(solutionStep.Name, solutionStep);
            solutionTypes.Add(solutionStep.Name, stepType);
        }

        public Step CreateStep(string name)
        {
            var descriptor = stepTypeDescriptor.Describe(solutionTypes[name]);
            var step = Activator.CreateInstance(solutionTypes[name], descriptor) as Step;
            step.Id = Guid.NewGuid().ToString();

            return step;
        }
    }
}
