using Automaton.Core.Enums;
using Automaton.Studio.Events;
using System;
using System.Collections.Generic;

namespace Automaton.Studio.Domain
{
    public class Definition
    {
        public string Id { get; set; }

        public int Version { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public WorkflowErrorHandling DefaultErrorBehavior { get; set; }

        public TimeSpan? DefaultErrorRetryInterval { get; set; }

        public List<Step> Steps { get; set; } = new List<Step>();

        public Flow Flow { get; set; }

        #region Events

        public event EventHandler<StepEventArgs> StepAdded;
        public event EventHandler<StepEventArgs> StepRemoved;

        #endregion

        public Definition()
        {
            Id = Guid.NewGuid().ToString();
            Name = "Untitled";
        }

        public void DeleteStep(Step step)
        {
            Steps.Remove(step);

            Flow.DeleteVariables(step.Variables);

            UpdateStepConnections();

            StepRemoved?.Invoke(this, new StepEventArgs(step));
        }

        public void FinalizeStep(Step step)
        {
            step.MarkAsFinal();
            step.Definition = this;

            UpdateStepConnections();

            StepAdded?.Invoke(this, new StepEventArgs(step));
        }

        public void UpdateStepConnections()
        {
            for(var i = 0; i < Steps.Count; i++)
            {
                Steps[i].NextStepId = i != Steps.Count - 1 ? Steps[i + 1].Id : null;
            }
        }
    }
}
