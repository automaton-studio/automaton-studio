using Automaton.Studio.Models;
using Automaton.Studio.Services;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages.Register;

public class UserRegisterViewModel
{
    private readonly UserRegisterService registrationService;

    public string UserName { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public string ConfirmPassword { get; set; }

    public bool AgreeWithTermsPrivacyPolicy { get; set; }

    public UserRegisterViewModel(UserRegisterService registrationService)
    {
        this.registrationService = registrationService;
    }

    public async Task Register()
    {

        var userDetails = new UserRegister
        {
            UserName= UserName,
            FirstName = FirstName,
            LastName = LastName,
            Email = Email,
            Password = Password
        };

        await registrationService.Register(userDetails);
    }
}
