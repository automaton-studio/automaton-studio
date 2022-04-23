using AutoMapper;
using Automaton.Studio.Models;
using Automaton.Studio.Services;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages.Login
{
    public class LoginViewModel
    {
        private readonly IMapper mapper;
        private readonly AuthenticationService authenticationService;

        public LoginModel LoginModel { get; set;  }
      
        public LoginViewModel
        (
            IMapper mapper,
            AuthenticationService authenticationService
        )
        {
            this.mapper = mapper;
            this.authenticationService = authenticationService;
            this.LoginModel = new LoginModel();
        }

        public async Task<bool> Login()
        {
            var loginDetails = mapper.Map<LoginCredentials>(LoginModel);
            var result = await authenticationService.Login(loginDetails);

            return result;
        }
    }
}
