using System;

namespace Automaton
{
    public abstract class Activity
    {
        /// <summary>
        /// The type name of this activity.
        /// </summary>
        public string Type { get; }

        /// <summary>
        /// Unique identifier of this activity within the workflow.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Name identifier of this activity.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Display name of this activity.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Description of this activity.
        /// </summary>
        public string Description { get; set; }

        public abstract void Execute();
    }
}
