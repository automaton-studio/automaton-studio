using Automaton.Studio.Models;
using Automaton.Studio.Resources;
using FluentValidation;

namespace Automaton.App.Account.Account;

public class FlowScheduleModelValidator : AbstractValidator<FlowScheduleModel>
{
    public FlowScheduleModelValidator()
    {
        RuleFor(x => x.RunnerIds).NotEmpty().WithMessage(Errors.RunnerRequiredToScheduleFlow);
    }
}
