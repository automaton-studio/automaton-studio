using FluentValidation;

namespace Automaton.Studio.Pages.Login
{
    public class LoginValidator : AbstractValidator<LoginModel>
    {
        public LoginValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage(Resources.Errors.UserNameRequired);
            RuleFor(x => x.Password).NotEmpty().WithMessage(Resources.Errors.PasswordRequired);
        }
    }
}
