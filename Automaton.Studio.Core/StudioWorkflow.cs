using AutoMapper;
using Elsa.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        #region Constructors

        public StudioWorkflow()
        {
            Name = "Untitled";
            DisplayName = "Untitled";
            Version = 1;
            Connections = new List<ConnectionDefinition>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Retrieves activity by index
        /// </summary>
        /// <param name="index">Activity index</param>
        /// <returns>Activity by index</returns>
        public StudioActivity GetActivity(int index)
        {
            return index >= 0 && index < Activities.Count ? Activities[index] : null;
        }

        /// <summary>
        /// Load activity to workflow.
        /// The difference from AddActivity is that UI is not refreshed
        /// (we don't call ActivityAdded event)
        /// </summary>
        /// <param name="activity">Activity to be Loaded to workflow</param>
        public void LoadActivity(StudioActivity activity)
        {
            activity.StudioWorkflow = this;

            Activities.Add(activity);
        }

        /// <summary>
        /// Add activity to workflow
        /// </summary>
        /// <param name="activity">Activity to be added to workflow</param>
        public void AddActivity(StudioActivity activity)
        {
            LoadActivity(activity);

            ActivityAdded?.Invoke(this, new ActivityEventArgs(activity));
        }

        /// <summary>
        /// Activity is pending and not final yet
        /// </summary>
        /// <param name="activity"></param>
        public void PendingActivity(StudioActivity activity)
        {
            activity.Pending();
        }

        /// <summary>
        /// The activity was created and it's final
        /// </summary>
        /// <param name="activity"></param>
        public void FinalizeActivity(StudioActivity activity)
        {
            activity.Finalize(this);

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

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
