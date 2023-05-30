using Automaton.Core.Models;
using Automaton.Studio.Domain;
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
        var success = Enum.TryParse(variable.Type, true, out VariableType type);

        if (!success)
        {
            return false;
        }

        return type switch
        {
            VariableType.String => string.IsNullOrEmpty(variable.Value?.ToString()),
            _ => false,
        };
    }
}
