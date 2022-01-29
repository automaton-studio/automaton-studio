using FluentValidation;
using System.Linq;

namespace Automaton.Studio.Components.NewDefinition
{
    public class NewDefinitionValidator : AbstractValidator<NewDefinitionModel>
    {
        public NewDefinitionValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(50).WithMessage(Resources.Errors.NameRequired);

            When(x => !string.IsNullOrEmpty(x.Name), () => {
                RuleFor(x => x).Must(NameIsUnique).WithMessage(Resources.Errors.DefinitionNameExists);
            });     
        }

        private bool NameIsUnique(NewDefinitionModel model)
        {
            return !model.ExistingNames.Any(x => x.ToLower() == model.Name.ToLower());
        }
    }
}
