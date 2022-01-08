using FluentValidation;

namespace Automaton.Studio.Steps.EmitLog
{
    public class EmitLogValidator : AbstractValidator<EmitLogActivity>
    {
        public EmitLogValidator()
        {
            RuleFor(x => x.Text).NotEmpty().MaximumLength(50).WithMessage("Text required");
        }
    }
}
