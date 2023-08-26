using Automaton.Core.Models;
using Automaton.Studio.Steps.Sequence;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Automaton.Studio.Domain;

public abstract class StudioStep : INotifyPropertyChanged
{
    private const int DefaultMargin = 10;

    protected virtual string StepClass { get; set; } = "designer-step";
    protected virtual string SelectedStepClass { get; set; } = "designer-step-selected";
    protected virtual string DisabledStepClass { get; set; } = "designer-step-disabled";

    #region Automaton.Core

    public string Id { get; set; }

    public string Name { get; set; }

    public string DisplayName { get; set; }

    public string Description { get; set; }

    public string MoreInfo { get; set; }

    public string Type { get; set; }

    public string Icon { get; set; }

    public string CancelCondition { get; set; }

    public bool HasProperties { get; set; } = true;

    public bool ShowVariables { get; set; } = true;

    public string NextStepId { get; set; }

    public string ParentId { get; set; }

    public bool IsNew { get; set; }

    public bool IsExecuting { get; private set; }

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

    public object GetInputValue(string name)
    {
        return Inputs[name].Value;
    }

    public void SetInputValue(string name, object value)
    {
        Inputs[name] = new StepVariable { Name = name, Value = value };
    }

    public object GetOutputValue(string name)
    {
        return Outputs[name].Value;
    }

    public void SetOutputValue(string name, object value)
    {
        SetOutputVariable(new StepVariable { Name = name, Value = value });
    }

    public void SetOutputVariable(StepVariable variable)
    {
        if (Outputs.ContainsKey(variable.Name))
        {
            Outputs[variable.Name] = variable;
        } 
        else
        {
            Outputs.Add(variable.Name, variable);
        }

        Flow.SetVariable(variable);
    }

    public void UpdateOutputVariable(string originalName, StepVariable variable)
    {
        if (Outputs.ContainsKey(originalName))
            Outputs.Remove(originalName);

        if (Outputs.ContainsKey(variable.Name))
        {
            Outputs[variable.Name] = variable;
        }
        else
        {
            Outputs.Add(variable.Name, variable);
        }

        if (Flow.VariableExists(originalName))
            Flow.DeleteVariable(originalName);

        Flow.SetVariable(variable);
    }

    public IEnumerable<StepVariable> GetOutputVariables()
    {
        return Outputs.Values;
    }

    public virtual void Select()
    {
        Class = SelectedStepClass;
    }

    public virtual void Unselect()
    {
        Class = StepClass;
    }

    public virtual void Complete()
    {
        IsNew = false;
        Definition.CompleteStep(this);
    }

    public virtual void Created()
    {
        IsNew = true;
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

    public void SetExecuting()
    {
        IsExecuting = true;
    }

    public void UnsetExecuting()
    {
        IsExecuting = false;
    }

    public int GetMargin()
    {
        var level = GetNestedLevel();

        var stepMargin = level * DefaultMargin;

        return stepMargin;
    }

    private int GetNestedLevel(int level = 1)
    {
        if (HasParent())
        {
            return Parent.GetNestedLevel(++level);
        }

        return level;
    }

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion
}
