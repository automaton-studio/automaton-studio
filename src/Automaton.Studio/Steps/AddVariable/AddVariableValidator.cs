using FluentValidation;

namespace Automaton.Studio.Steps.AddVariable
{
    public class AddVariableValidator : AbstractValidator<AddVariableStep>
    {
        public AddVariableValidator()
        {
            RuleFor(x => x.VariableName).NotEmpty().MaximumLength(256).WithMessage("Name required");
            RuleFor(x => x.VariableValue).NotNull().WithMessage("Variable required");
        }
    }
}
