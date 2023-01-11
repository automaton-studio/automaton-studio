using FluentValidation;

namespace Automaton.Studio.Pages.Register;

public class UserRegisterValidator : AbstractValidator<UserRegisterModel>
{
    public UserRegisterValidator()
    {
        RuleFor(x => x.UserName).NotEmpty().MaximumLength(256).WithMessage(Resources.Errors.UserNameRequired);
        RuleFor(x => x.Password).NotEmpty().MaximumLength(1024).WithMessage(Resources.Errors.PasswordRequired);
        RuleFor(x => x.ConfirmPassword).NotEmpty().MaximumLength(1024).WithMessage(Resources.Errors.ConfirmPasswordRequired);
        RuleFor(x => x.AgreeWithTermsPrivacyPolicy).Equal(true).WithMessage(Resources.Errors.AgreeWithTermsPrivacyPolicy);
    }
}
