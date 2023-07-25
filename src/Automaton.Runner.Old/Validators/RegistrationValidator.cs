using Automaton.Runner.Resources;
using Automaton.Runner.ViewModels;
using FluentValidation;

namespace Automaton.Runner.Validators;

public class RegistrationValidator : AbstractValidator<RegistrationViewModel>
{
    public RegistrationValidator()
    {
        RuleFor(x => x.RunnerName).NotEmpty().WithMessage(Errors.RunnerNameRequired);
        RuleFor(x => x.RunnerName).MaximumLength(50).WithMessage(Errors.RunnerMaxName);   
    }
}
