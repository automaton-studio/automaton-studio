using Automaton.Studio.Core.Metadata;
using Elsa;
using Elsa.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Automaton.Studio.Core
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

        #region Elsa Properties

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

        /// <summary>
        /// Activity index in the workflow's activities list
        /// </summary>
        public int Index => StudioWorkflow.Activities.IndexOf(this);

        /// <summary>
        /// Previous activity in the workflow's activities list
        /// </summary>
        public StudioActivity PreviousActivity => StudioWorkflow.GetActivity(Index - 1);

        /// <summary>
        /// Next activity in the workflow's activities list
        /// </summary>
        public StudioActivity NextActivity => StudioWorkflow.GetActivity(Index + 1);

        #endregion

        #region Events

        public event EventHandler<ActivityEventArgs> ConnectionAdded;
        public event EventHandler<ActivityEventArgs> ConnectionRemoved;

        #endregion

        #region Constructors

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

        #endregion

        #region Public Methods

        public void Select()
        {
            Class = SelectedActivityClass;
        }

        public void Pending()
        {
            PendingCreation = true;
        }

        public void Finalize(StudioWorkflow workflow)
        {
            StudioWorkflow = workflow;
            PendingCreation = false;
            UpdateConnection();
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

        #region Protected Methods

        protected ActivityDefinitionProperty GetProperty(string propertyName)
        {
            return Properties?.SingleOrDefault(x => x.Name == propertyName);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Updates workflow connections according with the changes of this activity
        /// </summary>
        public void UpdateConnection()
        {
            //---------------------------------------------------
            // 1. Handle the original position of the activity
            //---------------------------------------------------

            var connectionTo = StudioWorkflow.Connections.SingleOrDefault(x => x.TargetActivityId == ActivityId);
            var connectionFrom = StudioWorkflow.Connections.SingleOrDefault(x => x.SourceActivityId == ActivityId);

            // If in between two activities
            if (connectionTo != null && connectionFrom != null)
            {
                // Remove the two connections
                StudioWorkflow.Connections.Remove(connectionTo);
                StudioWorkflow.Connections.Remove(connectionFrom);

                // Create a new one
                var newConnection = new ConnectionDefinition(connectionTo.SourceActivityId, connectionFrom.TargetActivityId, OutcomeNames.Done);
                StudioWorkflow.Connections.Add(newConnection);
            }

            // If on the start
            if (connectionTo == null && connectionFrom != null)
            {
                StudioWorkflow.Connections.Remove(connectionFrom);
            }

            // If in the end
            if (connectionFrom == null && connectionTo != null)
            {
                StudioWorkflow.Connections.Remove(connectionTo);
            }

            //---------------------------------------------------
            // 2. Handle the new position of the activity
            //---------------------------------------------------

            // If in between two activities
            if (PreviousActivity != null && NextActivity != null)
            {
                // Break existing connection between activities
                var existingConnection = StudioWorkflow.Connections.SingleOrDefault(x => 
                    x.SourceActivityId == PreviousActivity.ActivityId && 
                    x.TargetActivityId == NextActivity.ActivityId);

                StudioWorkflow.Connections.Remove(existingConnection);

                // Create two new connections
                var connection1 = new ConnectionDefinition(PreviousActivity.ActivityId, ActivityId, OutcomeNames.Done);
                StudioWorkflow.Connections.Add(connection1);

                var connection2 = new ConnectionDefinition(ActivityId, NextActivity.ActivityId, OutcomeNames.Done);
                StudioWorkflow.Connections.Add(connection2);
            }

            // If on the start
            if (PreviousActivity == null && NextActivity != null)
            {
                var connection = new ConnectionDefinition(ActivityId, NextActivity.ActivityId, OutcomeNames.Done);
                StudioWorkflow.Connections.Add(connection);
            }

            // If in the end
            if (NextActivity == null && PreviousActivity != null)
            {
                var connection = new ConnectionDefinition(PreviousActivity.ActivityId, ActivityId, OutcomeNames.Done);
                StudioWorkflow.Connections.Add(connection);
            }
        }

        /// <summary>
        /// Adds a new connection to previous activity and updates connection to previous activity
        /// </summary>
        public void DeleteConnection()
        {
            var connectionTo = StudioWorkflow.Connections.SingleOrDefault(x => x.TargetActivityId == ActivityId);
            var connectionFrom = StudioWorkflow.Connections.SingleOrDefault(x => x.SourceActivityId == ActivityId);

            // If in between two activities
            if (connectionTo != null && connectionFrom != null)
            {
                // Remove the two connections
                StudioWorkflow.Connections.Remove(connectionTo);
                StudioWorkflow.Connections.Remove(connectionFrom);

                // Create a new one
                var newConnection = new ConnectionDefinition(connectionTo.SourceActivityId, connectionFrom.TargetActivityId, OutcomeNames.Done);
                StudioWorkflow.Connections.Add(newConnection);
            }

            // If on the start
            if (connectionTo == null && connectionFrom != null)
            {
                StudioWorkflow.Connections.Remove(connectionFrom);
            }

            // If in the end
            if (connectionFrom == null && connectionTo != null)
            {
                StudioWorkflow.Connections.Remove(connectionTo);
            }
        }

        /// <summary>
        /// Called when a connection is atached to this activity
        /// </summary>
        /// <param name="connection">Connection being attached to this activity</param>
        public virtual void ConnectionAttached(ConnectionDefinition connection)
        {
            // This is supposed to be implemented by activities that needs to
            // update connection after being attached.

            // As an example, Else activity needs to update the incoming conection and reattach it to
            // the corresponding If activity for OutcomeNames.False. The Else activity itself does not
            // have any connections pointing to it
        }

        #endregion

        #region Abstracts

        /// <summary>
        /// Get the view component type to use
        /// </summary>
        /// <returns></returns>
        public abstract Type GetDesignerComponent();

        /// <summary>
        /// Get the properties component type to use
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
