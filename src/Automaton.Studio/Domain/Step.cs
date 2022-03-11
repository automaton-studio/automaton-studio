using Automaton.Core.Enums;
using Automaton.Studio.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Runtime.CompilerServices;

namespace Automaton.Studio.Domain
{
    public abstract class Step : INotifyPropertyChanged
    {
        #region Constants

        private const string StepClass = "designer-step";
        private const string SelectedStepClass = "designer-step-selected";
        private const string DisabledStepClass = "designer-step-disabled";

        #endregion

        #region Members

        private bool isFinal;

        #endregion

        #region Properties

        public Definition Definition { get; set; }

        public Definition ActiveDefinition { get; set; }

        public IStepDescriptor Descriptor { get; set; }

        public IDictionary<string, object> InputsDictionary => Inputs;

        public string Class { get; set; }

        public string DisplayName { get; set; }

        #endregion

        #region Automaton.Core

        public string Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public string CancelCondition { get; set; }

        public WorkflowErrorHandling? ErrorBehavior { get; set; }

        public TimeSpan? RetryInterval { get; set; }

        public List<List<Step>> Do { get; set; } = new List<List<Step>>();

        public List<Step> CompensateWith { get; set; } = new List<Step>();

        public bool Saga { get; set; } = false;

        public string NextStepId { get; set; }

        public ExpandoObject Inputs { get; set; }

        public Dictionary<string, string> Outputs { get; set; }

        public IList<string> Variables { get; set; }

        #endregion

        public Step(IStepDescriptor descriptor)
        {
            Descriptor = descriptor;
            Name = descriptor.Name;
            DisplayName = descriptor.DisplayName;
            Type = descriptor.Type;
            Inputs = new ExpandoObject();
            Outputs = new Dictionary<string, string>();
            Variables = new List<string>();
            Class = StepClass;
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

        public void MarkAsFinal()
        {
            isFinal = true;
        }

        public bool IsFinal()
        {
            return isFinal;
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
