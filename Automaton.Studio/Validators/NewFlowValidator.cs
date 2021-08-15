using Automaton.Studio.Models;
using Automaton.Studio.Resources;
using Automaton.Studio.Services;
using FluentValidation;

namespace Automaton.Studio.Validators
{
    public class NewFlowValidator : AbstractValidator<FlowModel>
    {
        private readonly IFlowService flowService;

        public NewFlowValidator(IFlowService flowService)
        {
            this.flowService = flowService;

            RuleFor(x => x.Name).NotEmpty().MaximumLength(50).WithMessage(Errors.NameRequired);

            When(x => !string.IsNullOrEmpty(x.Name), () => {
                RuleFor(x => x.Name).Must(HasUniqueName).WithMessage(Errors.FlowNameExists);
            });     
        }

        private bool HasUniqueName(string name)
        {
            return flowService.IsUnique(name);
        }
    }
}
