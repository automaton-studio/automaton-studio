using Automaton.Runner.ViewModels;
using FluentValidation;

namespace Automaton.Runner.Validators
{
    public class RegistrationValidator : AbstractValidator<RegistrationViewModel>
    {
        public RegistrationValidator()
        {
            RuleFor(x => x.RunnerName).NotEmpty();
        }
    }
}
