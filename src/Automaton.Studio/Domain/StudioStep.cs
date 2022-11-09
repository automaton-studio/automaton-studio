using Automaton.Core.Enums;
using Automaton.Studio.Domain.Interfaces;
using Automaton.Studio.Events;
using Automaton.Studio.Pages.Designer.Components;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Automaton.Studio.Domain;

public abstract class StudioStep : INotifyPropertyChanged
{

    #region Members

    private bool isFinal;

    #endregion

    #region Events

    public event EventHandler<StepEventArgs> Finalize;
    public event EventHandler<StepEventArgs> Finalized;

    #endregion

    #region Properties

    protected string StepClass { get; set; } = "designer-step";
    protected string SelectedStepClass { get; set; } = "designer-step-selected";
    protected string DisabledStepClass { get; set; } = "designer-step-disabled";

    public StudioDefinition Definition { get; set; }

    public StudioFlow Flow => Definition.Flow;

    public string Class { get; set; }

    public string DisplayName { get; set; }

    public bool Hidden { get; set; }

    #endregion

    #region Automaton.Core

    public string Id { get; set; }

    public string Name { get; set; }

    public string Type { get; set; }

    public string Icon { get; set; }

    public string CancelCondition { get; set; }

    public WorkflowErrorHandling? ErrorBehavior { get; set; }

    public TimeSpan? RetryInterval { get; set; }

    public virtual bool HasProperties { get; set; } = true;

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

    public virtual void Setup(IStepDescriptor descriptor)
    {
        Id = Guid.NewGuid().ToString();
        Name = descriptor.Name;
        DisplayName = descriptor.DisplayName;
        Type = descriptor.Type;
        Icon = descriptor.Icon;
        StepClass = "designer-step";
        SelectedStepClass = "designer-step-selected";
        DisabledStepClass = "designer-step-disabled";
    }

    public virtual void Select()
    {
        Class = SelectedStepClass;
    }

    public virtual void Unselect()
    {
        Class = StepClass;
    }

    public virtual void SetSelectClass()
    {
        Class = SelectedStepClass;
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

    public bool IsVisible()
    {
        return !Hidden;
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

    public void InvokeFinalize()
    {
        Finalize?.Invoke(this, new StepEventArgs(this));
    }

    public void InvokeFinalized()
    {
        Finalized?.Invoke(this, new StepEventArgs(this));
    }

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion
}
