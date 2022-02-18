using FluentValidation;

namespace Automaton.Studio.Steps.EmitLog
{
    public class EmitLogValidator : AbstractValidator<EmitLogStep>
    {
        public EmitLogValidator()
        {
            RuleFor(x => x.Message).NotEmpty().MaximumLength(4000).WithMessage("Message required");
        }
    }
}
