namespace Automaton
{
    public class ActivityDefinitionProperty
    {
        /// <summary>
        /// The name of the property.
        /// </summary>
        public string Name { get; set; } = default!;

        /// <summary>
        /// Contains an expression
        /// </summary>
        public string Expression { get; set; }
    }
}
