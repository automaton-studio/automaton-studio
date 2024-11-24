using FluentValidation;

namespace Automaton.Studio.Steps.AddVariable;

public class AddVariableValidator : AbstractValidator<AddVariableStep>
{
    public AddVariableValidator()
    {
        RuleFor(x => x.Variable.Name).NotEmpty().WithMessage(Resources.Errors.NameRequired);
        RuleFor(x => x.Variable.Value).NotEmpty().WithMessage(Resources.Errors.ValueRequired);
    }
}
