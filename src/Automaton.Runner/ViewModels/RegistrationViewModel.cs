﻿using Automaton.Runner.Core.Services;
using Automaton.Runner.Enums;
using Automaton.Runner.Resources;
using Automaton.Runner.Services;
using Automaton.Runner.Validators;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Automaton.Runner.ViewModels
{
    public class RegistrationViewModel
    {
        private readonly HubService hubService;
        private readonly RegisterService registrationService;
        private readonly RegistrationValidator registrationValidator;
        private readonly ConfigService configService;

        public LoaderViewModel Loader { get; set; }
        public string RunnerName { get; set; }

        public RegistrationViewModel
        (
            LoaderViewModel loader,
            HubService hubService,
            RegisterService registrationService,
            ConfigService configService,
            RegistrationValidator registrationValidator
        )
        {
            this.hubService = hubService;
            this.Loader = loader;
            this.configService = configService;
            this.registrationService = registrationService;
            this.registrationValidator = registrationValidator;
        }

        public async Task<RunnerNavigation> Register()
        {
            try
            {
                if (!Validate())
                {
                    return RunnerNavigation.None;
                }

                Loader.StartLoading();

                await registrationService.Register(RunnerName);

                configService.RegisterRunner(RunnerName);

                await hubService.Connect(RunnerName);

                return RunnerNavigation.Dashboard;
            }
            catch
            {
                Loader.SetErrors(Errors.RegistrationError);
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

            var results = registrationValidator.Validate(this);

            if (results != null && results.Errors.Any())
            {
                Loader.SetErrors(string.Join(Environment.NewLine, results.Errors.Select(x => x.ErrorMessage).ToArray()));

                return false;
            }

            return true;
        }
    }
}