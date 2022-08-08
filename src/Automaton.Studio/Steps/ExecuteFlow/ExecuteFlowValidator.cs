using FluentValidation;

namespace Automaton.Studio.Steps.ExecuteFlow
{
    public class ExecuteFlowValidator : AbstractValidator<ExecuteFlowStep>
    {
        public ExecuteFlowValidator()
        {
            RuleFor(x => x.FlowId).NotEmpty().WithMessage(Resources.Errors.FlowRequired);
        }
    }
}
