using Automaton.Studio.Services;
using FluentValidation;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages.CustomSteps.Components.NewCustomStep;

public class NewCustomStepValidator : AbstractValidator<NewCustomStepModel>
{
    private readonly CustomStepsService customStepsService;

    public NewCustomStepValidator(CustomStepsService customStepsService)
    {
        this.customStepsService = customStepsService;

        RuleFor(x => x.Name).NotEmpty().MaximumLength(50).WithMessage(Resources.Errors.NameRequired);
        RuleFor(x => x.DisplayName).NotEmpty().MaximumLength(120).WithMessage(Resources.Errors.DisplayNameRequired);

        When(x => !string.IsNullOrEmpty(x.Name), () =>
        {
            RuleFor(x => x.Name).Must(HasUniqueName).WithMessage(Resources.Errors.CustomStepNameExists);
        });
    }

    private bool HasUniqueName(string name)
    {
        var isUnique = !Task.Run(() => customStepsService.Exists(name)).Result;

        return isUnique;
    }
}
