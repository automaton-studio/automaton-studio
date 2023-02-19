using Automaton.Studio.Server.Models;
using Automaton.Studio.Server.Services;
using FluentValidation;

namespace Automaton.Studio.Server.Validators
{
    public class RegisterRunnerValidator : AbstractValidator<RegisterRunnerDetails>
    {
        private readonly RunnerService runnerService;

        public RegisterRunnerValidator(RunnerService runnerService)
        {
            this.runnerService = runnerService;

            RuleFor(x => x.Name).NotEmpty().MaximumLength(50).WithMessage(Resources.Errors.NameRequired);

            When(x => !string.IsNullOrEmpty(x.Name), () => {
                RuleFor(x => x).Must(NameIsUnique).WithMessage(Resources.Errors.RunnerExists);
            });     
        }

        private bool NameIsUnique(RegisterRunnerDetails command)
        {
            /// You should NOT use asynchronous rules when using ASP.NET automatic validation
            /// as ASP.NET’s validation pipeline is not asynchronous.
            /// https://docs.fluentvalidation.net/en/latest/aspnet.html
            var nameIsUnique = !runnerService.Exists(command.Name);

            return nameIsUnique;
        }
    }
}
