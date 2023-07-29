using Automaton.App.Authentication.Services;
using System.Threading.Tasks;

namespace Automaton.Studio.Shared
{
    public class MainLayoutViewModel
    {
        private readonly AuthenticationService authenticationService;

        public MainLayoutViewModel(AuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        public async Task Logout()
        {
            await authenticationService.Logout();
        }
    }
}
