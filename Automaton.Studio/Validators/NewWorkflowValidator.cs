using Automaton.Studio.Models;
using Automaton.Studio.Resources;
using Automaton.Studio.Specifications;
using Elsa.Persistence;
using FluentValidation;
using System.Threading.Tasks;

namespace Automaton.Studio.Validators
{
    public class NewWorkflowValidator : AbstractValidator<NewWorkflow>
    {
        private readonly IWorkflowDefinitionStore workflowDefinitionStore;

        public NewWorkflowValidator(IWorkflowDefinitionStore workflowDefinitionStore)
        {
            this.workflowDefinitionStore = workflowDefinitionStore;

            RuleFor(x => x.Name).NotEmpty().MaximumLength(50).WithMessage(Errors.WorkflowNameRequired);

            When(x => !string.IsNullOrEmpty(x.Name), () => {
                RuleFor(x => x.Name).Must(HaveUniqueName).WithMessage(Errors.WorkflowNameExists);
            });     
        }

        private bool HaveUniqueName(string name)
        {
            var task = Task.Run(() => workflowDefinitionStore.CountAsync(new WorkflowDefinitionNameSpecification(name)));

            return task.Result == 0;
        }
    }
}
