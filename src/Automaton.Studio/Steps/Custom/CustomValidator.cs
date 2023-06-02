using Automaton.Core.Enums;
using Automaton.Core.Models;
using FluentValidation;

namespace Automaton.Studio.Steps.Custom;

public class CustomValidator : AbstractValidator<CustomStep>
{
    public CustomValidator()
    {
        When(x => x.CodeInputVariables.Any(), () =>
        {
            RuleFor(x => x.CodeInputVariables).Must(CodeInputHaveValidValues).WithMessage("Input values not valid");
        });
    }

    private bool CodeInputHaveValidValues(IList<StepVariable> variables)
    {
        return !variables.Any(x => InvalidVariableValue(x));
    }

    private static bool InvalidVariableValue(StepVariable variable)
    {
        return variable.Type switch
        {
            VariableType.String => string.IsNullOrEmpty(variable.Value?.ToString()),
            _ => false,
        };
    }
}
