using Automaton.Studio.Conductor.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Runtime.CompilerServices;

namespace Automaton.Studio.Conductor
{
    public abstract class Step : INotifyPropertyChanged
    {
        #region Constants

        private const string StepClass = "designer-activity";
        private const string SelectedStepClass = "designer-activity-selected";
        private const string DisabledStepClass = "designer-activity-disabled";

        #endregion

        #region Members

        private bool pendingCreation = true;

        #endregion

        #region Properties

        public IStepDescriptor Descriptor { get; set; }

        public IDictionary<string, object> InputsDictionary => Inputs;

        public Definition StudioWorkflow { get; set; }

        public string Class { get; set; }

        public string DisplayName { get; set; }

        public string Type { get; set; }

        #endregion

        #region Conductor Properties

        public string StepType { get; set; }

        public string Id { get; set; }

        public string Name { get; set; }

        public string CancelCondition { get; set; }

        public DefinitionErrorHandling? ErrorBehavior { get; set; }

        public TimeSpan? RetryInterval { get; set; }

        public List<List<Step>> Do { get; set; } = new List<List<Step>>();

        public List<Step> CompensateWith { get; set; } = new List<Step>();

        public bool Saga { get; set; } = false;

        public string NextStepId { get; set; }

        public ExpandoObject Inputs { get; set; } = new ExpandoObject();

        public Dictionary<string, string> Outputs { get; set; } = new Dictionary<string, string>();

        public Dictionary<string, string> SelectNextStep { get; set; } = new Dictionary<string, string>();

        #endregion

        public Step(IStepDescriptor descriptor)
        {
            Descriptor = descriptor;
            Name = descriptor.Name;
            DisplayName = descriptor.DisplayName;
            Type = descriptor.Type;
            StepType = descriptor.Type;
        }

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

        public void MarkAsCreated()
        {
            pendingCreation = false;
        }

        public bool IsPendingCreation()
        {
            return pendingCreation;
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
