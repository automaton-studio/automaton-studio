using Automaton.Studio.Server.Core.Commands;
using Automaton.Studio.Server.Resources;
using Automaton.Studio.Server.Services;
using FluentValidation;

namespace Automaton.Studio.Server.Application.Commands.Validators
{
    public class RegisterRunnerCommandValidator : AbstractValidator<RegisterRunnerCommand>
    {
        private readonly RunnerService runnerService;

        public RegisterRunnerCommandValidator(RunnerService runnerService)
        {
            this.runnerService = runnerService;

            RuleFor(x => x.Name).NotEmpty().MaximumLength(50).WithMessage(Errors.NameRequired);

            When(x => !string.IsNullOrEmpty(x.Name), () => {
                RuleFor(x => x).Must(NameIsUnique).WithMessage(Errors.RunnerExists);
            });     
        }

        private bool NameIsUnique(RegisterRunnerCommand command)
        {
            /// You should NOT use asynchronous rules when using ASP.NET automatic validation
            /// as ASP.NET’s validation pipeline is not asynchronous.
            /// https://docs.fluentvalidation.net/en/latest/aspnet.html
            var nameIsUnique = Task.Run(() => runnerService.DoNotExists(command.Name, CancellationToken.None)).Result;

            return nameIsUnique;
        }
    }
}
