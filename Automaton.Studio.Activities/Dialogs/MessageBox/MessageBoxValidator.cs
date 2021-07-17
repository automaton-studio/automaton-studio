using Automaton.Studio.Activities.Dialogs.MessageBox;
using Automaton.Studio.Activities.Resources;
using FluentValidation;

namespace Automaton.Studio.Activities.Console.WriteLine
{
    public class MessageBoxValidator : AbstractValidator<MessageBoxActivity>
    {
        public MessageBoxValidator()
        {
            RuleFor(x => x.Text).NotEmpty().MaximumLength(50).WithMessage(Errors.TextRequired);
        }
    }
}
