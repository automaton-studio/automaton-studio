using FluentValidation;

namespace Automaton.Studio.Steps.ExecuteWorkflow
{
    public class ExecuteWorkflowValidator : AbstractValidator<ExecuteWorkflowStep>
    {
        public ExecuteWorkflowValidator()
        {
            RuleFor(x => x.FlowId).NotEmpty().WithMessage(Resources.Errors.FlowRequired);
        }
    }
}
