using Automaton.Studio.Services;
using FluentValidation;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages.Flows.Components.NewFlow
{
    public class NewFlowValidator : AbstractValidator<NewFlowModel>
    {
        private readonly FlowsService flowsService;

        public NewFlowValidator(FlowsService flowService)
        {
            this.flowsService = flowService;

            RuleFor(x => x.Name).NotEmpty().MaximumLength(50).WithMessage("Name is required");

            When(x => !string.IsNullOrEmpty(x.Name), () =>
            {
                RuleFor(x => x.Name).Must(HasUniqueName).WithMessage(Resources.Errors.FlowNameExists);
            });
        }

        private bool HasUniqueName(string name)
        {
            var isUnique = !Task.Run(() => flowsService.Exists(name)).Result;

            return isUnique;
        }
    }
}
