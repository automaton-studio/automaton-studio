using AutoMapper;
using Elsa;
using Elsa.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Automaton.Studio.Core
{
    public class StudioWorkflow : INotifyPropertyChanged
    {
        #region Elsa Properties

        public string Tag { get; set; }
        public bool IsLatest { get; set; }
        public bool IsPublished { get; set; }
        public bool DeleteCompletedInstances { get; set; }
        public WorkflowPersistenceBehavior PersistenceBehavior { get; set; }
        public bool IsSingleton { get; set; }
        public WorkflowContextOptions ContextOptions { get; set; }
        public Variables CustomAttributes { get; set; }
        public int Version { get; set; }
        public string Description { get; set; }
        public string DisplayName { get; set; }
        public string Name { get; set; }
        public string TenantId { get; set; }
        public string VersionId { get; }
        public string DefinitionId { get; set; }
        public Variables Variables { get; set; }
        public ICollection<ConnectionDefinition> Connections { get; set; }
        // Replaced Elsa Activities with own Studio Activities

        #endregion

        #region Public Properties

        private IList<StudioActivity> activities = new List<StudioActivity>();

        [IgnoreMap]
        public IList<StudioActivity> Activities
        {
            get => activities;

            private set
            {
                activities = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Events

        public event EventHandler<ActivityEventArgs> ActivityAdded;
        public event EventHandler<ActivityEventArgs> ActivityRemoved;

        #endregion

        public StudioWorkflow()
        {
            Name = "Untitled";
            DisplayName = "Untitled";
            Version = 1;
            Connections = new List<ConnectionDefinition>();
        }

        #region Public Methods

        /// <summary>
        /// Add activity to workflow
        /// </summary>
        /// <param name="activity">Activity to be added to workflow</param>
        public void AddActivity(StudioActivity activity)
        {
            activity.StudioWorkflow = this;

            Activities.Add(activity);

            ActivityAdded?.Invoke(this, new ActivityEventArgs(activity));
        }

        /// <summary>
        /// Activity is pending and not final yet
        /// </summary>
        /// <param name="activity"></param>
        public void PendingActivity(StudioActivity activity)
        {
            activity.StudioWorkflow = this;
            activity.PendingCreation = true;
        }

        /// <summary>
        /// The activity was created and it's final
        /// </summary>
        /// <param name="activity"></param>
        public void FinalizeActivity(StudioActivity activity)
        {
            activity.StudioWorkflow = this;
            activity.PendingCreation = false;

            NewConnection(activity);

            ActivityAdded?.Invoke(this, new ActivityEventArgs(activity));
        }

        /// <summary>
        /// Remove activity from workflow
        /// </summary>
        /// <param name="activity">Activity to be removed from workflow</param>
        public bool RemoveActivity(StudioActivity activity)
        {
            var result = Activities.Remove(activity);

            ActivityRemoved?.Invoke(this, new ActivityEventArgs(activity));

            return result;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Create a new connection for activity
        /// </summary>
        /// <param name="activity">Activity to create connection for</param>
        private void NewConnection(StudioActivity activity)
        {
            // Current activity
            var activityIndex = Activities.IndexOf(activity);

            // Return if not found
            if (activityIndex < 0)
            {
                return;
            }

            // Previous activity
            var previousActivityIndex = activityIndex > 0 ? activityIndex - 1 : -1;
            var previousActivity = previousActivityIndex >= 0 ? Activities[previousActivityIndex] : null;

            // Next activity
            var nextActivityIndex = activityIndex < Activities.Count - 1 ? activityIndex + 1 : -1;
            var nextActivity = nextActivityIndex >= 0 ? Activities[nextActivityIndex] : null;

            // TODO! Outcome should not be hardcoded.
            var activityConnection = previousActivity != null ?
                new ConnectionDefinition(previousActivity.ActivityId, activity.ActivityId, OutcomeNames.Done) :
                null;

            // Add connection if there is a previous activity
            if (activityConnection != null)
            {
                Connections.Add(activityConnection);
            }

            // If there is a next activity, update its connection to point to the new activity as its source
            if (nextActivity != null)
            {
                var nextActivityConnection = Connections.SingleOrDefault(x => x.TargetActivityId == nextActivity.ActivityId);
                if (nextActivityConnection != null)
                {
                    nextActivityConnection.SourceActivityId = activity.ActivityId;
                }
            }
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
