using Automaton.Core.Attributes;
using Automaton.Core.Models;
using Automaton.Core.Scripting;
using Automaton.Core.Parsers;
using Newtonsoft.Json.Linq;

namespace Automaton.Steps;

public class CustomStep : WorkflowStep
{
    private const string ContentType = @"text/x-python";

    private readonly ScriptEngineHost scriptHost;

    [IgnorePropertyParsing(true)]
    public string? Code { get; set; }

    public IList<StepVariable> CodeInputVariables { get; set; }

    [IgnorePropertyParsing(true)]
    public IList<StepVariable> CodeOutputVariables { get; set; }

    public CustomStep(ScriptEngineHost scriptHost)
    {
        this.scriptHost = scriptHost;
        CodeInputVariables = new List<StepVariable>();
        CodeOutputVariables = new List<StepVariable>();
    }

    protected override Task<ExecutionResult> RunAsync(StepExecutionContext context)
    {
        var resource = new ScriptResource()
        {
            ContentType = ContentType,
            Content = Code
        };

        var inputVariablesDictionary = CodeInputVariables.ToDictionary(x => x.Name, x => x.Value);

        var scriptVariables = scriptHost.Execute(resource, inputVariablesDictionary);

        foreach (var variable in CodeOutputVariables)
        {
            if (scriptVariables.ContainsKey(variable.Name))
            {
                variable.Value = scriptVariables[variable.Name].ToString();
            }
        }

        return Task.FromResult(ExecutionResult.Next());
    }

    protected override void SetProperties(StepExecutionContext context)
    {
        Code = Inputs[nameof(Code)]?.Value?.ToString();

        IList<StepVariable>? codeInputVariables;

        if (Inputs[nameof(CodeInputVariables)].Value is JArray codeInputArray)
        {
            codeInputVariables = codeInputArray.ToObject<IList<StepVariable>>();
        }
        else
        {
            codeInputVariables = Inputs[nameof(CodeInputVariables)].Value as IList<StepVariable>;
        }

        foreach (var variable in codeInputVariables)
        {
            var variableValue = ExpressionParser.Parse(variable.Value, context.Workflow);
            variable.Value = variableValue;

            CodeInputVariables.Add(variable);
        }

        if (Inputs[nameof(CodeOutputVariables)].Value is JArray codeOutputArray)
        {
            CodeOutputVariables = codeOutputArray.ToObject<IList<StepVariable>>();
        }
        else
        {
            CodeOutputVariables = Inputs[nameof(CodeOutputVariables)].Value as IList<StepVariable>;
        }
    }
}
