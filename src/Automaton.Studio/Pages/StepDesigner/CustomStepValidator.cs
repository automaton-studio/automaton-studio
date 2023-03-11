using Automaton.Studio.Domain;
using FluentValidation;

namespace Automaton.Studio.Pages.StepDesigner
{
    public class CustomStepValidator : AbstractValidator<CustomStep>
    {
        public CustomStepValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(256).WithMessage(Resources.Errors.NameRequired);
            RuleFor(x => x.DisplayName).NotEmpty().MaximumLength(256).WithMessage(Resources.Errors.DisplayNameRequired);
        }
    }

}
