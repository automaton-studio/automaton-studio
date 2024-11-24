using Automaton.Core.Enums;
using Newtonsoft.Json.Linq;

namespace Automaton.Core.Models;

public class StepVariable
{
    public string? Id { get; set; }
    public required string? Name { get; set; }
    public VariableType Type { get; set; } = VariableType.String;
    public virtual object? Value { get; set; }
    public string? Description { get; set; }

    public Type GetValueType()
    {
        return Type switch
        {
            VariableType.String or VariableType.Text => typeof(string),
            VariableType.Boolean => typeof(bool),
            VariableType.Number => typeof(decimal),
            VariableType.Date => typeof(DateTime),
            _ => typeof(string),
        };
    }

    public T GetValue<T>()
    {
        if (Value is JArray inputArray)
        {
            // We need to return back the value of the same instance 
            // so it can be updated from the UI.
            // ToObject<T>() create a new object which is not
            // bound to the UI controls, unless we assign it to existingVariable.Value 
            Value = inputArray.ToObject<T>();

            return GetValue<T>();
        }

        if (Value is JObject inputObject)
        {
            // We need to return back the value of the same instance 
            // so it can be updated from the UI.
            // ToObject<T>() create a new object which is not
            // bound to the UI controls, unless we assign it to existingVariable.Value 
            Value = inputObject.ToObject<T>();

            return GetValue<T>();
        }

        return (T)Value;
    }
}
