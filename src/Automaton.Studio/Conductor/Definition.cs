using Automaton.Studio.Events;
using System;
using System.Collections.Generic;

namespace Automaton.Studio.Conductor
{
    public class Definition
    {
        #region Conductor

        public string Id { get; set; }

        public int Version { get; set; }

        public string Description { get; set; }

        public WorkflowErrorHandling DefaultErrorBehavior { get; set; }

        public TimeSpan? DefaultErrorRetryInterval { get; set; }

        public List<Step> Steps { get; set; } = new List<Step>();

        #endregion

        public event EventHandler<StepEventArgs> ActivityAdded;
        public event EventHandler<StepEventArgs> ActivityRemoved;

        /// <summary>
        /// Reletes activity from workflow
        /// </summary>
        /// <param name="activity">Activity to be deleted from workflow</param>
        public void DeleteActivity(Step activity)
        {
            activity.DeleteConnections();

            Steps.Remove(activity);

            ActivityRemoved?.Invoke(this, new StepEventArgs(activity));
        }
    }
}
