using System;
using System.Collections.Generic;
using System.Linq;

namespace Automaton.Studio
{
    public class AutomatonOptions
    {
        private readonly IDictionary<string, Type> studioActivityTypes = new Dictionary<string, Type>();
        private readonly IDictionary<string, Type> elsaActivityTypes = new Dictionary<string, Type>();

        public IReadOnlyCollection<Type> AutomatonActivities => studioActivityTypes.Values.ToList().AsReadOnly();

        public void AddStudioActivityType(string automatonName, Type type)
        {
            studioActivityTypes[automatonName] = type;
        }

        public void AddElsaActivityType(string elsaName, Type type)
        {
            elsaActivityTypes[elsaName] = type;
        }

        public Type GetStudioActivityType(string name)
        {
            return studioActivityTypes[name];
        }

        public Type GetElsaActivityType(string name)
        {
            return elsaActivityTypes[name];
        }
    }
}