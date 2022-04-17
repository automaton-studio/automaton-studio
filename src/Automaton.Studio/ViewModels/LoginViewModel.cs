using Automaton.Studio.Models;
using Automaton.Studio.Services.Interfaces;
using System.Threading.Tasks;

namespace Automaton.Studio.ViewModels
{
    public class LoginViewModel : ILoginViewModel
    {
        private readonly IAuthenticationService authenticationService;

        public LoginModel Model { get; set;  }
      
        public LoginViewModel
        (
            IAuthenticationService authenticationService
        )
        {
            this.authenticationService = authenticationService;
            this.Model = new LoginModel();
        }

        public async Task<bool> Login()
        {
            var result = await authenticationService.Login(Model);

            return result;
        }
    }
}
