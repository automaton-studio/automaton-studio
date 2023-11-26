using Automaton.Core.Models;

namespace Automaton.Steps;

public class If : Sequence
{
    public StepVariable Expression { get; set; }

    protected override Task<ExecutionResult> RunAsync(StepExecutionContext context)
    {
        Expression = Inputs[nameof(Expression)];

        var result = ExpressionIsTrue(context.Workflow);

        NextStepId = result ? NextStepId : SequenceEndStepId;

        return Task.FromResult(ExecutionResult.Next());
    }

    private bool ExpressionIsTrue(Workflow workflow)
    {
        var parsedExpression = StepVariableParser.Parse(Expression, workflow);

        return parsedExpression is bool result && result;
    }
}
