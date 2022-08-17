using Automaton.Studio.Services;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages.Account;

public class AccountViewModel
{
    private readonly AuthenticationService authenticationService;
  
    public AccountViewModel(AuthenticationService authenticationService)
    {
        this.authenticationService = authenticationService;
    }

    public async Task Logout()
    {
        await authenticationService.Logout();
    }
}
