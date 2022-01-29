using Automaton.Studio.Conductor;
using Automaton.Studio.Events;
using Automaton.Studio.Factories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Automaton.Studio.Domain
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

        #region Events

        public event EventHandler<StepEventArgs> StepAdded;
        public event EventHandler<StepEventArgs> StepRemoved;

        #endregion

        public void DeleteStep(Step step)
        {
            //step.DeleteConnections();

            Steps.Remove(step);

            StepRemoved?.Invoke(this, new StepEventArgs(step));
        }

        public void FinalizeStep(Step step)
        {
            step.MarkAsFinal();

            StepAdded?.Invoke(this, new StepEventArgs(step));
        }

        public void UpdateStepConnections()
        {
            for(var i = 0; i < Steps.Count - 1; i++)
            {
                var step = Steps[i];
                step.NextStepId = Steps[i + 1].Id;
            }

            var lastStep = Steps.Last();
            lastStep.NextStepId = null;
        }
    }
}
