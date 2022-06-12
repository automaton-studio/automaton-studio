using Automaton.Client.Auth.Models;
using Automaton.Runner.Core.Services;
using Automaton.Runner.Enums;
using Automaton.Runner.Resources;
using Automaton.Runner.Services;
using Automaton.Runner.Validators;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Automaton.Runner.ViewModels;

public class LoginViewModel
{
    private readonly ConfigService configService;
    private readonly AuthenticationService authenticationService;
    private readonly LoginValidator loginValidator;

    public LoaderViewModel Loader { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public bool HasErrors => Loader.HasErrors;

    public LoginViewModel(
        ConfigService configService,
        AuthenticationService authenticationService,
        LoaderViewModel loader,
        LoginValidator loginValidator)
    {
        this.configService = configService;
        this.authenticationService = authenticationService;
        this.Loader = loader;
        this.loginValidator = loginValidator;
    }

    public async Task<RunnerNavigation> Login()
    {
        try
        {
            if (!Validate())
            {
                return RunnerNavigation.None;
            }

            Loader.StartLoading();

            await authenticationService.Login(new LoginDetails(UserName, Password));

            if (configService.AppConfig.IsRunnerRegistered())
            {
                return RunnerNavigation.Dashboard;
            }
            else
            {
                return RunnerNavigation.Registration;
            }
        }
        catch (Exception ex)
        {
            Loader.SetErrors(Errors.AuthenticationError);
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
