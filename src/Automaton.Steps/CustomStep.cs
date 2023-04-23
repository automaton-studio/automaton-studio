using Automaton.Core.Attributes;
using Automaton.Core.Models;
using Automaton.Core.Scripting;
using Automaton.Core.Parsers;

namespace Automaton.Steps;

public class CustomStep : WorkflowStep
{
    private const string ContentType = @"text/x-python";

    private readonly ScriptEngineHost scriptHost;

    [IgnorePropertyParsing(true)]
    public string? Code { get; set; }

    public IList<CustomStepVariable> CodeInputVariables { get; set; }

    [IgnorePropertyParsing(true)]
    public IList<CustomStepVariable> CodeOutputVariables { get; set; }

    public CustomStep(ScriptEngineHost scriptHost)
    {
        this.scriptHost = scriptHost;
        CodeInputVariables = new List<CustomStepVariable>();
        CodeOutputVariables = new List<CustomStepVariable>();
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
                context.Workflow.Variables[variable.Name] = variable.Value;
            }
        }

        return Task.FromResult(ExecutionResult.Next());
    }

    protected override void SetProperties(StepExecutionContext context)
    {
        Code = Inputs[nameof(Code)]?.Value?.ToString();

        var codeInputVariables = Inputs[nameof(CodeInputVariables)].Value as IList<CustomStepVariable>;

        foreach (var variable in codeInputVariables)
        {
            var variableValue = ExpressionParser.Parse(variable.Value, context.Workflow);
            variable.Value = variableValue;

            CodeInputVariables.Add(variable);
        }


    }
}
