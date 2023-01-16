using Automaton.Studio.Models;
using Automaton.Studio.Services;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages.Account;

public class AccountViewModel
{
    private readonly AuthenticationService authenticationService;
    private readonly UserAccountService userAccountService;

    public string UserName { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public string ConfirmPassword { get; set; }

    public AccountViewModel(AuthenticationService authenticationService, 
        UserAccountService userAccountService)
    {
        this.authenticationService = authenticationService;
        this.userAccountService = userAccountService;
    }

    public async Task Logout()
    {   
        await authenticationService.Logout();
    }

    public async Task UpdateUserProfile()
    {
        var userUpdate = new UserUpdate
        {
            UserName = UserName,
            FirstName = FirstName,
            LastName = LastName,
            Email = Email
        };

        await userAccountService.UpdateUserProfile(userUpdate);
    }
}
