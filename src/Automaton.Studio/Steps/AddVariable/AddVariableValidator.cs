using FluentValidation;

namespace Automaton.Studio.Steps.AddVariable
{
    public class AddVariableValidator : AbstractValidator<AddVariableStep>
    {
        public AddVariableValidator()
        {
            RuleFor(x => x.VariableName).NotEmpty().MaximumLength(256).WithMessage(Resources.Errors.NameRequired);
            RuleFor(x => x.VariableValue).NotNull().WithMessage(Resources.Errors.ValueRequired);
        }
    }
}
