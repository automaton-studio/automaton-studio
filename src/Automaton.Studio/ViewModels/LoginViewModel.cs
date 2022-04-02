using AutoMapper;
using Automaton.Studio.Domain;
using Automaton.Studio.Models;
using Automaton.Studio.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Automaton.Studio.ViewModels
{
    public class LoginViewModel : ILoginViewModel
    {
        private readonly IMapper mapper;
        private ILoginService loginService;

        public LoginModel Model { get; set;  }
      
        public LoginViewModel
        (
            ILoginService loginService,
            IMapper mapper
        )
        {
            this.loginService = loginService;
            this.mapper = mapper;
            this.Model = new LoginModel();
        }

        public Task Login()
        {
            throw new NotImplementedException();
        }
    }
}
