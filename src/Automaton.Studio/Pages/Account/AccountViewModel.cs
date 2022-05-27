using Automaton.Client.Auth.Interfaces;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages.Account
{
    public class AccountViewModel
    {
        private readonly IAuthenticationService authenticationService;
      
        public AccountViewModel(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        public async Task Logout()
        {
            await authenticationService.Logout();
        }
    }
}
