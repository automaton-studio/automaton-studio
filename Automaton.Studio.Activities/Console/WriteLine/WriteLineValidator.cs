using Automaton.Studio.Activities.Resources;
using FluentValidation;

namespace Automaton.Studio.Activities.Console.WriteLine
{
    public class WriteLineValidator : AbstractValidator<WriteLineActivity>
    {
        public WriteLineValidator()
        {
            RuleFor(x => x.Text).NotEmpty().MaximumLength(50).WithMessage(Errors.TextRequired);
        }
    }
}
