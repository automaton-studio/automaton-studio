using Automaton.Core.Enums;
using Automaton.Studio.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Automaton.Studio.Domain;

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

    public string Class { get; set; }

    public string DisplayName { get; set; }      

    #endregion

    #region Automaton.Core

    public string Id { get; set; }

    public string Name { get; set; }

    public string Type { get; set; }

    public string Icon { get; set; }

    public string CancelCondition { get; set; }

    public WorkflowErrorHandling? ErrorBehavior { get; set; }

    public TimeSpan? RetryInterval { get; set; }

    public List<List<StudioStep>> Do { get; set; } = new List<List<StudioStep>>();

    public List<StudioStep> CompensateWith { get; set; } = new List<StudioStep>();

    public bool Saga { get; set; } = false;

    public string NextStepId { get; set; }

    public IDictionary<string, object> Inputs { get; set; } = new Dictionary<string, object>();

    public IDictionary<string, object> Outputs { get; set; } = new Dictionary<string, object>();

    #endregion

    public StudioStep()
    {
        Class = StepClass;
    }

    public abstract Type GetDesignerComponent();

    public abstract Type GetPropertiesComponent();

    public void Setup(IStepDescriptor descriptor)
    {
        Id = Guid.NewGuid().ToString();
        Name = descriptor.Name;
        DisplayName = descriptor.DisplayName;
        Type = descriptor.Type;
        Icon = descriptor.Icon;
    }

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
        if (!Outputs.ContainsKey(key))
        {
            Outputs.Add(key, value);
        }            

        Flow.SetVariable(key, value);
    }

    public IEnumerable<string> GetVariableNames()
    {
        return Outputs.Select(x => x.Key);
    }

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion
}
