using FluentValidation;

namespace Automaton.Studio.Steps.AddVariable;

public class AddVariableValidator : AbstractValidator<AddVariableStep>
{
    public AddVariableValidator()
    {

        RuleFor(x => x.VariableValue).NotEmpty().WithMessage(Resources.Errors.ValueRequired);
    }
}
