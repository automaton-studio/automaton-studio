using Automaton.Studio.Activities.Resources;
using FluentValidation;

namespace Automaton.Studio.Activities.Conditionals.If
{
    public class IfValidator : AbstractValidator<IfActivity>
    {
        public IfValidator()
        {
            RuleFor(x => x.Condition).NotEmpty().WithMessage(Errors.TextRequired);
        }
    }
}
