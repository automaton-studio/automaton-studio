using Automaton.Studio.Services;
using FluentValidation;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages.CustomSteps.Components.NewCustomStep;

public class NewCustomStepValidator : AbstractValidator<NewCustomStepModel>
{
    private readonly FlowsService flowsService;

    public NewCustomStepValidator(FlowsService flowService)
    {
        this.flowsService = flowService;

        RuleFor(x => x.Name).NotEmpty().MaximumLength(50).WithMessage(Resources.Errors.NameRequired);

        When(x => !string.IsNullOrEmpty(x.Name), () =>
        {
            RuleFor(x => x.Name).Must(HasUniqueName).WithMessage(Resources.Errors.FlowNameExists);
        });
    }

    private bool HasUniqueName(string name)
    {
        var isUnique = !Task.Run(() => flowsService.Exists(name)).Result;

        return isUnique;
    }
}
