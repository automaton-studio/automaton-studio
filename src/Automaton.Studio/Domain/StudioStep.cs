using Automaton.Core.Enums;
using Automaton.Studio.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Runtime.CompilerServices;

namespace Automaton.Studio.Domain
{
    public abstract class StudioStep : INotifyPropertyChanged
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

        public StudioDefinition Definition { get; set; }

        public StudioFlow Flow => Definition.Flow;

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

        public List<List<StudioStep>> Do { get; set; } = new List<List<StudioStep>>();

        public List<StudioStep> CompensateWith { get; set; } = new List<StudioStep>();

        public bool Saga { get; set; } = false;

        public string NextStepId { get; set; }

        public ExpandoObject Inputs { get; set; }

        public IList<string> Variables { get; set; }

        #endregion

        public StudioStep(IStepDescriptor descriptor)
        {
            Descriptor = descriptor;
            Name = descriptor.Name;
            DisplayName = descriptor.DisplayName;
            Type = descriptor.Type;
            Inputs = new ExpandoObject();
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

        public void SetVariable(string key, object value)
        {
            if (!Variables.Contains(key))
            {
                Variables.Add(key);
            }            

            Flow.SetVariable(key, value);
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
