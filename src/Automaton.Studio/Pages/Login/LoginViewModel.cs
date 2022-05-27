using AutoMapper;
using Automaton.Client.Auth.Interfaces;
using Automaton.Client.Auth.Models;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages.Login
{
    public class LoginViewModel
    {
        private readonly IMapper mapper;
        private readonly IAuthenticationService authenticationService;

        public LoginModel LoginDetails { get; set;  } = new LoginModel();

        public LoginViewModel
        (
            IMapper mapper,
            IAuthenticationService authenticationService
        )
        {
            this.mapper = mapper;
            this.authenticationService = authenticationService;
        }

        public async Task Login()
        {
            var loginDetails = mapper.Map<LoginDetails>(LoginDetails);
            await authenticationService.Login(loginDetails);
        }
    }
}
