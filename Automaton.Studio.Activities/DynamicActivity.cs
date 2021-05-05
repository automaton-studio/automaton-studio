using Elsa.Models;
using System;
using System.Collections.Generic;

namespace Automaton.Studio.Activities
{
    /// <summary>
    /// A base class for all activity instances
    /// </summary>
    public abstract class DynamicActivity
    {
        #region Elsa Activity properties

        public string ActivityId { get; set; }
        public string Type { get; set; }
        public string? Name { get; set; }
        public string? DisplayName { get; set; }
        public string? Description { get; set; }
        public bool PersistWorkflow { get; set; }
        public bool LoadWorkflowContext { get; set; }
        public bool SaveWorkflowContext { get; set; }
        public bool PersistOutput { get; set; }
        public ICollection<ActivityDefinitionProperty> Properties { get; set; }

        #endregion

        #region Abstracts

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

        #endregion
    }
}
