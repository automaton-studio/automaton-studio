using Automaton.Core.Attributes;
using Automaton.Core.Models;
using Automaton.Core.Parsers;
using Automaton.Core.Scripting;
using Newtonsoft.Json.Linq;

namespace Automaton.Steps;

public class CustomStep : WorkflowStep
{
    private const string ContentType = @"text/x-python";

    private readonly ScriptEngineHost scriptHost;

    [IgnorePropertyParsing(true)]
    public string? Code { get; set; }

    public IList<StepVariable> CodeInputVariables { get; set; }

    public CustomStep(ScriptEngineHost scriptHost)
    {
        this.scriptHost = scriptHost;
        CodeInputVariables = new List<StepVariable>();
    }

    protected override Task<ExecutionResult> RunAsync(StepExecutionContext context)
    {
        try
        {
            this.scriptHost.ScriptTextWritten += OnScriptTextWritten;

            var resource = new ScriptResource()
            {
                ContentType = ContentType,
                Content = Code
            };

            var inputVariablesDictionary = CodeInputVariables.ToDictionary(x => x.Name, x => x.Value);

            var scriptVariables = scriptHost.Execute(resource, inputVariablesDictionary);

            foreach (var variable in Outputs)
            {
                if (scriptVariables.ContainsKey(variable.Value.Id))
                {
                    variable.Value.Value = scriptVariables[variable.Value.Id].ToString();
                    context.Workflow.SetVariable(variable.Value);
                }
            }

            return Task.FromResult(ExecutionResult.Next());
        }
        finally
        {
            scriptHost.ScriptTextWritten -= OnScriptTextWritten;
        }     
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
    }

    private void OnScriptTextWritten(object sender, string text)
    {
        logger.Information("{Text}", text);
    }
}
