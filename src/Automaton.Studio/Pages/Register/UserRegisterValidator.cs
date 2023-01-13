using FluentValidation;

namespace Automaton.Studio.Pages.Register;

public class UserRegisterValidator : AbstractValidator<UserRegisterViewModel>
{
    public UserRegisterValidator()
    {
        RuleFor(x => x.UserName).NotEmpty().MaximumLength(256).WithMessage(Resources.Errors.UserNameRequired);
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(256).WithMessage(Resources.Errors.FirstNameRequired);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(256).WithMessage(Resources.Errors.LastNameRequired);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(256).WithMessage(Resources.Errors.EmailRequired);
        RuleFor(x => x.Password).NotEmpty().MaximumLength(1024).WithMessage(Resources.Errors.PasswordRequired);
        RuleFor(x => x.ConfirmPassword).NotEmpty().MaximumLength(1024).WithMessage(Resources.Errors.ConfirmPasswordRequired);
        RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage(Resources.Errors.ConfirmPasswordSameAsPassword);
        RuleFor(x => x.AgreeWithTermsPrivacyPolicy).Equal(true).WithMessage(Resources.Errors.AgreeWithTermsPrivacyPolicy);
    }
}
