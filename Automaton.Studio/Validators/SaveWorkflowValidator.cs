using Automaton.Studio.Core;
using Automaton.Studio.Resources;
using Automaton.Studio.Specifications;
using Elsa.Persistence;
using FluentValidation;
using System.Threading.Tasks;

namespace Automaton.Studio.Validators
{
    public class SaveWorkflowModelValidator : AbstractValidator<StudioWorkflow>
    {
        private readonly IWorkflowDefinitionStore workflowDefinitionStore;

        public SaveWorkflowModelValidator(IWorkflowDefinitionStore workflowDefinitionStore)
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
