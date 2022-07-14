using FluentValidation;

namespace Automaton.Studio.Steps.ExecutePython
{
    public class ExecutePythonValidator : AbstractValidator<ExecutePythonStep>
    {
        public ExecutePythonValidator()
        {
            //RuleFor(x => x.VariableName).NotEmpty().MaximumLength(256).WithMessage("Name required");
            //RuleFor(x => x.VariableValue).NotNull().WithMessage("Value required");
        }
    }
}
