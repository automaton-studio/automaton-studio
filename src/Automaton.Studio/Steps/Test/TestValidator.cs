using FluentValidation;

namespace Automaton.Studio.Steps.Test;

public class TestValidator : AbstractValidator<TestStep>
{
    public TestValidator()
    {
        RuleFor(x => x.Message).NotEmpty().MaximumLength(4000).WithMessage(Resources.Errors.MessageRequired);
    }
}
