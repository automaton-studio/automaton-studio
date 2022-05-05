using AutoMapper;
using Automaton.Client.Auth.Models;
using Automaton.Client.Auth.Services;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages.Login
{
    public class LoginViewModel
    {
        private readonly IMapper mapper;
        private readonly AuthenticationService authenticationService;

        public LoginModel LoginCredentials { get; set;  } = new LoginModel();

        public LoginViewModel
        (
            IMapper mapper,
            AuthenticationService authenticationService
        )
        {
            this.mapper = mapper;
            this.authenticationService = authenticationService;
        }

        public async Task<bool> Login()
        {
            try
            {
                var loginCredentials = mapper.Map<LoginCredentials>(LoginCredentials);
                await authenticationService.Login(loginCredentials);
            }
            catch (AuthenticationException ex)
            {
                return false;
            }           

            return true;
        }
    }
}
