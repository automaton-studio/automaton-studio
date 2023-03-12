using FluentValidation;

namespace Automaton.Studio.Pages.FlowDesigner.Components.NewVariable;

public class VariableValidator : AbstractValidator<VariableModel>
{
    public VariableValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(50).WithMessage(Resources.Errors.NameRequired);

        When(x => !string.IsNullOrEmpty(x.Name), () => {
            RuleFor(x => x).Must(NameIsUnique).WithMessage(Resources.Errors.NameExists);
        });     
    }

    private bool NameIsUnique(VariableModel model)
    {
        return !model.ExistingNames.Any(x => x.ToLower() == model.Name.ToLower());
    }
}
