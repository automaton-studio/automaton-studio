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

        public string Name { get; set; }

        public string Description { get; set; }

        public DefinitionErrorHandling DefaultErrorBehavior { get; set; }

        public TimeSpan? DefaultErrorRetryInterval { get; set; }

        public List<Step> Steps { get; set; } = new List<Step>();

        #endregion

        public event EventHandler<StepEventArgs> StepAdded;
        public event EventHandler<StepEventArgs> StepRemoved;

        public void DeleteStep(Step step)
        {
            step.DeleteConnections();

            Steps.Remove(step);

            StepRemoved?.Invoke(this, new StepEventArgs(step));
        }

        public void FinalizeStep(Step step)
        {
            step.MarkAsCreated();

            StepAdded?.Invoke(this, new StepEventArgs(step));
        }
    }
}
