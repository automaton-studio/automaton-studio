using Automaton.Studio.Activities.Resources;
using Automaton.Studio.Models;
using FluentValidation;

namespace Automaton.Studio.Validators
{
    public class WriteLineValidator : AbstractValidator<WriteLineModel>
    {
        public WriteLineValidator()
        {
            RuleFor(x => x.Text).NotEmpty().MaximumLength(50).WithMessage(Errors.WorkflowNameRequired);
        }
    }
}
