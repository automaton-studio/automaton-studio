﻿using Automaton.Client.Auth.Models;
using Automaton.Client.Auth.Services;
using Automaton.Runner.Core.Services;
using Automaton.Runner.Enums;
using Automaton.Runner.Resources;
using Automaton.Runner.Validators;
using System;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace Automaton.Runner.ViewModels
{
    public class LoginViewModel
    {
        private readonly ConfigService configService;
        private readonly HubService hubService;
        private readonly AuthenticationService authenticationService;
        private readonly LoginValidator loginValidator;

        #region Properties

        public LoaderViewModel Loader { get; set; }

        public string UserName { get; set; }
        public string Password { get; set; }

        #endregion

        #region Constructors

        public LoginViewModel(
            ConfigService configService,
            AuthenticationService authenticationService,
            HubService hubService,
            LoaderViewModel loader,
            LoginValidator loginValidator)
        {
            this.configService = configService;
            this.authenticationService = authenticationService;
            this.hubService = hubService;
            this.Loader = loader;
            this.loginValidator = loginValidator;
        }

        #endregion

        public async Task<RunnerNavigation> Login()
        {
            try
            {
                if (!Validate())
                {
                    return RunnerNavigation.None;
                }

                await authenticationService.Login(new LoginCredentials(UserName, Password));

                Loader.StartLoading();

                if (configService.UserConfig.IsRunnerRegistered())
                {
                    await hubService.Connect(configService.UserConfig.RunnerName);

                    return RunnerNavigation.Dashboard;
                }
                else
                {
                    return RunnerNavigation.Registration;
                }
            }
            catch (AuthenticationException ex)
            {
                Loader.SetErrors(Errors.AuthenticationError);
            }
            catch (Exception ex)
            {
                Loader.SetErrors(Errors.ApplicationError);
            }
            finally
            {
                Loader.StopLoading();
            }

            return RunnerNavigation.None;
        }

        private bool Validate()
        {
            Loader.ClearErrors();

            var results = loginValidator.Validate(this);

            if (results != null && results.Errors.Any())
            {
                Loader.SetErrors(string.Join(Environment.NewLine, results.Errors.Select(x => x.ErrorMessage).ToArray()));
                
                return false;
            }

            return true;
        }
    }
}