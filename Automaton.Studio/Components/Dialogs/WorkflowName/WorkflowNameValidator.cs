using Automaton.Studio.Resources;
using FluentValidation;
using System.Linq;

namespace Automaton.Studio.Components.Dialogs.WorkflowName
{
    public class WorkflowNameValidator : AbstractValidator<WorkflowNameModel>
    {
        public WorkflowNameValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(50).WithMessage(Errors.NameRequired);

            When(x => !string.IsNullOrEmpty(x.Name), () => {
                RuleFor(x => x).Must(NameIsUnique).WithMessage(Errors.WorkflowNameExists);
            });     
        }

        private bool NameIsUnique(WorkflowNameModel model)
        {
            return !model.ExistingNames.Any(x => x.ToLower() == model.Name.ToLower());
        }
    }
}
