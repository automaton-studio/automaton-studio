using System;
using System.Collections.Generic;

namespace Automaton.Studio.Activity
{
    public class AutomatonOptions
    {
        private readonly List<Type> types = new List<Type>();

        public IReadOnlyCollection<Type> Types => types.AsReadOnly();

        public void Add(Type type)
        {
            types.Add(type);
        }
    }
}