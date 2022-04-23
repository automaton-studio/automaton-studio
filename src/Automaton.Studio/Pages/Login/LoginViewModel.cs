using AutoMapper;
using Automaton.Studio.Models;
using Automaton.Studio.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages.Login
{
    public class LoginViewModel
    {
        private readonly IMapper mapper;
        private readonly AuthenticationService authenticationService;

        public LoginModel LoginCredentials { get; set;  } = new LoginModel();
        public Dictionary<string, List<string>> Errors { get; set; } = new Dictionary<string, List<string>>();

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
            ClearErrors();

            var loginCredentials = mapper.Map<LoginCredentials>(LoginCredentials);
            var result = await authenticationService.Login(loginCredentials);

            if (!result)
            {
                AddError(nameof(Resources.Errors.LoginFailed), Resources.Errors.LoginFailed);
            }

            return result;
        }

        private void AddError(string key, string error)
        {
            if (!Errors.ContainsKey(key))
            {
                Errors.Add(key, new List<string> { error });
            }
        }

        private void ClearErrors()
        {
            Errors.Clear();
        }
    }
}
