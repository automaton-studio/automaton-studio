using FluentValidation;

namespace Automaton.Studio.Steps;

public class StepSettingsValidator : AbstractValidator<StepSettingsModel>
{
    public StepSettingsValidator()
    {
        //RuleFor(x => x.DisplayName).NotEmpty().MaximumLength(50).WithMessage(Resources.Errors.NameRequired);
    }
}
