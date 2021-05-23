using System;
using System.Collections.Generic;
using System.Linq;

namespace Automaton.Studio.Activity
{
    public class AutomatonOptions
    {
        private readonly IDictionary<string, Type> automatonTypes = new Dictionary<string, Type>();
        private readonly IDictionary<string, Type> elsaTypes = new Dictionary<string, Type>();

        public IReadOnlyCollection<Type> AutomatonActivities => automatonTypes.Values.ToList().AsReadOnly();

        public void AddAutomatonActivity(string automatonName, Type type)
        {
            automatonTypes[automatonName] = type;
        }

        public void AddElsaActivity(string elsaName, Type type)
        {
            elsaTypes[elsaName] = type;
        }

        public Type GetAutomatonActivity(string name)
        {
            return automatonTypes[name];
        }

        public Type GetElsaActivity(string name)
        {
            return elsaTypes[name];
        }
    }
}