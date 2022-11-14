using FluentValidation;

namespace Automaton.Studio.Steps.TestReport;

public class TestReportValidator : AbstractValidator<TestReportStep>
{
    public TestReportValidator()
    {
        RuleFor(x => x.Expression).NotEmpty().MaximumLength(4000).WithMessage(Resources.Errors.MessageRequired);
    }
}
