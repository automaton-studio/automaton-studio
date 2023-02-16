using Automaton.Core.Models;
using Automaton.Studio.Domain.Interfaces;
using Automaton.Studio.Events;
using Automaton.Studio.Steps.Sequence;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Automaton.Studio.Domain;

public abstract class StudioStep : INotifyPropertyChanged
{
    public event EventHandler<StepEventArgs> Finalize;
    public event EventHandler<StepEventArgs> Finalized;
    public event EventHandler<StepEventArgs> Created;

    protected string StepClass { get; set; } = "designer-step";
    protected string SelectedStepClass { get; set; } = "designer-step-selected";
    protected string DisabledStepClass { get; set; } = "designer-step-disabled";

    #region Automaton.Core

    public string Id { get; set; }

    public string Name { get; set; }

    public string DisplayName { get; set; }

    public string Description { get; set; }

    public string MoreInfo { get; set; }

    public string Type { get; set; }

    public string Icon { get; set; }

    public string CancelCondition { get; set; }

    public virtual bool HasProperties { get; set; } = true;

    public bool ShowVariables { get; set; } = true;

    public string NextStepId { get; set; }

    public string ParentId { get; set; }

    public bool IsFinal { get; set; }

    public IDictionary<string, StepVariable> Inputs { get; set; } = new Dictionary<string, StepVariable>();

    public IDictionary<string, StepVariable> Outputs { get; set; } = new Dictionary<string, StepVariable>();

    #endregion

    public StudioDefinition Definition { get; set; }

    public StudioFlow Flow => Definition.Flow;

    public string Class { get; set; }

    public bool Hidden { get; set; }

    public SequenceStep Parent
    {
        get
        {
            var parent = Definition.Steps.SingleOrDefault(x => x.Id == ParentId);

            return parent as SequenceStep;
        }
    }

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
        Description = descriptor.Description;
        MoreInfo = descriptor.MoreInfo;
        Type = descriptor.Type;
        Icon = descriptor.Icon;
        StepClass = "designer-step";
        SelectedStepClass = "designer-step-selected";
        DisabledStepClass = "designer-step-disabled";
    }

    public void SetInputVariable(string name, object value)
    {
        Inputs[name] = new StepVariable { Value = value };
    }

    public object GetInputVariable(string name)
    {
        return Inputs[name].Value;
    }

    public bool InputVariableExists(string name)
    {
        return Inputs.ContainsKey(name);
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


    public bool IsVisible()
    {
        return !Hidden;
    }

    public bool HasParent()
    {
        return !string.IsNullOrEmpty(ParentId);
    }

    public bool HasVariables()
    {
        return Outputs.Any();
    }

    public void SetVariable(StepVariable variable)
    {
        if (Outputs.ContainsKey(variable.Key))
        {
            Outputs[variable.Key] = variable;
        }
        else
        {
            Outputs.Add(variable.Key, variable);
        }

        Flow.SetVariable(variable);
    }

    public IEnumerable<StepVariable> GetVariables()
    {
        return Outputs.Values;
    }

    public void InvokeCreated()
    {
        Created?.Invoke(this, new StepEventArgs(this));
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
