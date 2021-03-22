using System;

namespace Automaton.Studio.Activities
{
    /// <summary>
    /// A base class for all activity instances
    /// </summary>
    public abstract class ActivityBase
    {
        /// <summary>
        /// An Id 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Product name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Abstract method to get the view component type to use
        /// </summary>
        /// <returns></returns>
        public abstract Type GetViewComponent();

        /// <summary>
        /// Abstract method to get the properties component type to use
        /// </summary>
        /// <returns></returns>
        public abstract Type GetPropertiesComponent();
    }
}
