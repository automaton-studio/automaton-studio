using FluentValidation;

namespace Automaton.Studio.Pages.Login
{
    public class LoginValidator : AbstractValidator<LoginModel>
    {
        public LoginValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().MaximumLength(256).WithMessage(Resources.Errors.UserNameRequired);
            RuleFor(x => x.Password).NotEmpty().MaximumLength(1024).WithMessage(Resources.Errors.PasswordRequired);
        }
    }
}
