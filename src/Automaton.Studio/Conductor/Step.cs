using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Runtime.CompilerServices;

namespace Automaton.Studio.Conductor
{
    public abstract class Step : INotifyPropertyChanged
    {
        #region Conductor

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

        #endregion

        #region Custom properties

        public IStepDescriptor Descriptor { get; set; }

        public Step(IStepDescriptor descriptor)
        {
            Descriptor = descriptor;
        }

        private const string StepClass = "designer-activity";
        private const string SelectedStepClass = "designer-activity-selected";
        private const string DisabledStepClass = "designer-activity-disabled";

        public Definition StudioWorkflow { get; set; }

        public bool PendingCreation { get; set; }

        public string Class { get; set; }

        public abstract Type GetDesignerComponent();

        public abstract Type GetPropertiesComponent();

        public void Select()
        {
            Class = SelectedStepClass;
        }

        public void Unselect()
        {
            Class = StepClass;
        }

        public bool IsSelected()
        {
            return Class == SelectedStepClass;
        }

        public void UpdateConnections()
        {
            //UpdateExistingConnections();

            //UpdateNewConnections();
        }

        public void DeleteConnections()
        {
            //UpdateExistingConnections();
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
