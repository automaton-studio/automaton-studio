using FluentValidation;

namespace Automaton.Studio.Steps.AddVariable;

public class AddVariableValidator : AbstractValidator<AddVariableStep>
{
    public AddVariableValidator()
    {
        RuleFor(x => x.VariableName).NotEmpty().WithMessage(Resources.Errors.NameRequired);

        When(x => !string.IsNullOrEmpty(x.VariableName), () =>
        {
            RuleFor(x => x).Must(NameIsUnique).WithMessage(Resources.Errors.NameExists);
        });

        RuleFor(x => x.VariableValue).NotEmpty().WithMessage(Resources.Errors.ValueRequired);
    }

    private bool NameIsUnique(AddVariableStep step)
    {
        var flowVariables = step.Definition.Flow.Variables;

        var nameIsUnique = !flowVariables.ContainsKey(step.VariableName);

        return nameIsUnique;
    }
}
