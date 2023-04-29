using Automaton.Core.Models;
using FluentValidation;

namespace Automaton.Studio.Steps.Custom;

public class CustomValidator : AbstractValidator<CustomStep>
{
    public CustomValidator()
    {
        RuleFor(x => x.Code).NotEmpty().WithMessage("Code required");

        When(x => x.CodeInputVariables.Any(), () =>
        {
            RuleFor(x => x.CodeInputVariables).Must(HaveValidVariableName).WithMessage("Input variable name not valid");
        });

        When(x => x.CodeOutputVariables.Any(), () =>
        {
            RuleFor(x => x.CodeOutputVariables).Must(HaveValidVariableName).WithMessage("Output variable name not valid");
        });
    }

    private bool HaveValidVariableName(IList<StepVariable> variables)
    {
        return !variables.Any(x => string.IsNullOrEmpty(x.Name));
    }
}
