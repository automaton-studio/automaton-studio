using Automaton.Studio.Activity.Metadata;
using Elsa.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Automaton.Studio.Activity
{
    /// <summary>
    /// A base class for all activity instances
    /// </summary>
    public abstract class StudioActivity
    {
        #region Members

        private IActivityTypeDescriber activityDescriber;

        #endregion

        #region Elsa Activity properties

        public string ActivityId { get; set; }
        public string? Name { get; set; }
        public string? DisplayName { get; set; }
        public string? Description { get; set; }
        public bool PersistWorkflow { get; set; }
        public bool LoadWorkflowContext { get; set; }
        public bool SaveWorkflowContext { get; set; }
        public bool PersistOutput { get; set; }
        public ICollection<ActivityDefinitionProperty>? Properties { get; set; }

        #endregion

        #region Public Properties

        public ActivityDescriptor Descriptor { get; set; }

        #endregion

        public StudioActivity(IActivityTypeDescriber activityDescriber)
        {
            this.activityDescriber = activityDescriber;
            ActivityId = Guid.NewGuid().ToString();
            Properties = new List<ActivityDefinitionProperty>();
            Descriptor = activityDescriber.Describe(this.GetType());
        }

        protected ActivityDefinitionProperty GetDefinitionProperty(string propertyName)
        {
            return Properties?.SingleOrDefault(x => x.Name == propertyName);
        }

        #region Abstracts

        /// <summary>
        /// Abstract method to get the view component type to use
        /// </summary>
        /// <returns></returns>
        public abstract Type GetDesignerComponent();

        /// <summary>
        /// Abstract method to get the properties component type to use
        /// </summary>
        /// <returns></returns>
        public abstract Type GetDialogComponent();

        #endregion
    }
}
