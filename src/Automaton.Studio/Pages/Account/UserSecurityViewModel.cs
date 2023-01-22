using Automaton.Studio.Services;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages.Account;

public class UserSecurityViewModel
{
    private readonly UserAccountService userAccountService;

    public string OldPassword { get; set; }

    public string NewPassword { get; set; }

    public UserSecurityViewModel(UserAccountService userAccountService)
    {
        this.userAccountService = userAccountService;
    }

    public async Task UpdateUserPassword()
    {
        var userPassword = new Models.UserPassword
        {
            OldPassword = OldPassword,
            NewPassword = NewPassword
        };

        await userAccountService.UpdateUserPassword(userPassword);
    }
}
