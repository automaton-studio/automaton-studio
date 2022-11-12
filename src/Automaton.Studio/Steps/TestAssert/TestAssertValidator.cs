using FluentValidation;

namespace Automaton.Studio.Steps.TestAssert;

public class TestAssertValidator : AbstractValidator<TestAssertStep>
{
    public TestAssertValidator()
    {
        RuleFor(x => x.Expression).NotEmpty().MaximumLength(4000).WithMessage(Resources.Errors.MessageRequired);
    }
}
