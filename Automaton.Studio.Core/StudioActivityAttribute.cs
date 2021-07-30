using Elsa.Attributes;
using System;

namespace Automaton.Studio.Core
{
    [AttributeUsage(AttributeTargets.Class)]
    public class StudioActivityAttribute : ActivityAttribute
    {
        public string Name { get; set; }
        public string Icon { get; set; }
    }
}