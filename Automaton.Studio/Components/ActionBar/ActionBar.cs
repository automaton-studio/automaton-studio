using System;

namespace Automaton.Studio.Components.ActionBar
{
    public abstract class ActionBar
    {
        /// <summary>
        /// Returns the view component type to render
        /// </summary>
        /// <returns>View component type to render</returns>
        public abstract Type GetViewComponent();
    }
}
