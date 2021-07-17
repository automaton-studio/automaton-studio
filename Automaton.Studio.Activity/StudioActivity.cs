using Automaton.Studio.Activity.Metadata;
using Elsa.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Automaton.Studio.Activity
{
    /// <summary>
    /// A base class for all activity instances
    /// </summary>
    public abstract class StudioActivity : INotifyPropertyChanged, IEquatable<StudioActivity>
    {
        #region Constants

        private const string ActivityClass = "designer-activity";
        private const string SelectedActivityClass = "designer-activity-selected";
        private const string DisabledActivityClass = "designer-activity-disabled";

        #endregion

        #region Members

        private IActivityTypeDescriber activityDescriber;

        #endregion

        #region Elsa Activity Properties

        public string ActivityId { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool PersistWorkflow { get; set; }
        public bool LoadWorkflowContext { get; set; }
        public bool SaveWorkflowContext { get; set; }
        public bool PersistOutput { get; set; }
        public ICollection<ActivityDefinitionProperty> Properties { get; set; }

        #endregion

        #region Public Properties

        public ActivityDescriptor Descriptor { get; set; }

        /// <summary>
        /// The StudioWorkflow activity is part of
        /// </summary>
        public StudioWorkflow StudioWorkflow { get; set; }

        /// <summary>
        /// Specifies if the activity is pending creation
        /// </summary>
        public bool PendingCreation { get; set; }

        /// <summary>
        /// Activity designer class
        /// </summary>
        public string Class { get; set; }

        #endregion

        public StudioActivity(IActivityTypeDescriber activityDescriber)
        {
            this.activityDescriber = activityDescriber;
            ActivityId = Guid.NewGuid().ToString();
            Properties = new List<ActivityDefinitionProperty>();
            Descriptor = this.activityDescriber.Describe(this.GetType());
            Class = ActivityClass;

            var attribute = GetType().GetCustomAttribute<StudioActivityAttribute>(false);
            Name = attribute.Name ?? GetType().Name;
            Type = attribute.Type ?? GetType().Name;
            DisplayName = attribute.DisplayName ?? GetType().Name;
            Description = attribute.Description ?? string.Empty;
        }

        #region Public Properties

        public void Select()
        {
            Class = SelectedActivityClass;
        }

        public void Unselect()
        {
            Class = ActivityClass;
        }

        public bool IsSelected()
        {
            return Class == SelectedActivityClass;
        }

        #endregion

        #region Protected Properties

        protected ActivityDefinitionProperty GetProperty(string propertyName)
        {
            return Properties?.SingleOrDefault(x => x.Name == propertyName);
        }

        #endregion

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
        public abstract Type GetPropertiesComponent();

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region IEquatable

        public bool Equals(StudioActivity other)
        {
            return ActivityId == other.ActivityId;
        }

        #endregion
    }
}
