using Automaton.Studio.Pages.Account;
using FluentValidation;

namespace Automaton.Studio.Pages.Login;

public class UserSecurityValidator : AbstractValidator<UserSecurityViewModel>
{
    public UserSecurityValidator()
    {
        RuleFor(x => x.OldPassword).NotEmpty().MaximumLength(1024).WithMessage(Resources.Errors.OldPasswordRequired);
        RuleFor(x => x.NewPassword).NotEmpty().MaximumLength(1024).WithMessage(Resources.Errors.NewPasswordRequired);
    }
}
