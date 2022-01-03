using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Automaton.Studio.Conductor
{
    public class Step
    {
        public string StepType { get; set; }

        public string Id { get; set; }

        public string Name { get; set; }

        public string CancelCondition { get; set; }

        public WorkflowErrorHandling? ErrorBehavior { get; set; }

        public TimeSpan? RetryInterval { get; set; }

        public List<List<Step>> Do { get; set; } = new List<List<Step>>();

        public List<Step> CompensateWith { get; set; } = new List<Step>();

        public bool Saga { get; set; } = false;

        public string NextStepId { get; set; }

        public ExpandoObject Inputs { get; set; } = new ExpandoObject();

        public Dictionary<string, string> Outputs { get; set; } = new Dictionary<string, string>();

        public Dictionary<string, string> SelectNextStep { get; set; } = new Dictionary<string, string>();

        #region Custom properties

        private const string ActivityClass = "designer-activity";
        private const string SelectedActivityClass = "designer-activity-selected";
        private const string DisabledActivityClass = "designer-activity-disabled";

        public Definition StudioWorkflow { get; set; }

        public bool PendingCreation { get; set; }

        /// <summary>
        /// Activity designer class
        /// </summary>
        public string Class { get; set; }

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

        /// <summary>
        /// Updates workflow connections according with the changes of this activity
        /// </summary>
        public void UpdateConnections()
        {
            //UpdateExistingConnections();

            //UpdateNewConnections();
        }

        /// <summary>
        /// Updates activity connections when deleted.
        /// </summary>
        public void DeleteConnections()
        {
            //UpdateExistingConnections();
        }

        #endregion
    }
}
