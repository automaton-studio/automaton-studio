using Automaton.Core.Models;
using FluentValidation;
using System.Collections.Generic;
using System.Linq;

namespace Automaton.Studio.Steps.ExecuteFlow
{
    public class ExecuteFlowValidator : AbstractValidator<ExecuteFlowStep>
    {
        public ExecuteFlowValidator()
        {
            RuleFor(x => x.FlowId).NotEmpty().WithMessage(Resources.Errors.FlowRequired);

            When(x => x.InputVariables.Any(), () =>
            {
                RuleFor(x => x.InputVariables).Must(HaveValidVariableName).WithMessage("Input variable name not valid");
            });

            When(x => x.OutputVariables.Any(), () =>
            {
                RuleFor(x => x.OutputVariables).Must(HaveValidVariableName).WithMessage("Output variable name not valid");
            });
        }

        private bool HaveValidVariableName(IList<Variable> variables)
        {
            return !variables.Any(x => string.IsNullOrEmpty(x.Name));
        }
    }
}
