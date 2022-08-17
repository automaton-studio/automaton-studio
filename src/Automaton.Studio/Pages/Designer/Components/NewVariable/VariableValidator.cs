using FluentValidation;
using System.Linq;

namespace Automaton.Studio.Pages.Designer.Components.NewVariable;

public class VariableValidator : AbstractValidator<VariableModel>
{
    public VariableValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(50).WithMessage(Resources.Errors.NameRequired);

        When(x => !string.IsNullOrEmpty(x.Name), () => {
            RuleFor(x => x).Must(NameIsUnique).WithMessage(Resources.Errors.VariableNameExists);
        });     
    }

    private bool NameIsUnique(VariableModel model)
    {
        return !model.ExistingNames.Any(x => x.ToLower() == model.Name.ToLower());
    }
}
