namespace Automaton.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property )
]
    public class IgnorePropertyParsing : Attribute
    {
        public bool Ignore { get; private set; }

        public IgnorePropertyParsing(bool ignore)
        {
            this.Ignore = ignore;
        }
    }
}
