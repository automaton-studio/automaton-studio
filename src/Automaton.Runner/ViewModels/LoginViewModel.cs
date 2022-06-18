using Automaton.Client.Auth.Models;
using Automaton.Runner.Resources;
using Automaton.Runner.Services;
using Automaton.Runner.Validators;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Automaton.Runner.ViewModels;

public class LoginViewModel
{
    private readonly AuthenticationService authenticationService;
    private readonly LoginValidator loginValidator;

    public LoaderViewModel Loader { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }

    public LoginViewModel(
        AuthenticationService authenticationService,
        LoaderViewModel loader,
        LoginValidator loginValidator)
    {
        this.authenticationService = authenticationService;
        this.Loader = loader;
        this.loginValidator = loginValidator;
    }

    public async Task<bool> Login()
    {
        try
        {
            if (!Validate())
            {
                return false;
            }

            StartLoading();

            await LoginUser();
        }
        catch
        {
            SetLoadingErrors();
            return false;
        }
        finally
        {
            StopLoading();
        }

        return true;
    }

    private async Task LoginUser()
    {
        await authenticationService.Login(new LoginDetails(UserName, Password));
    }

    private bool Validate()
    {
        Loader.ClearErrors();

        var result = loginValidator.Validate(this);

        if (result.Errors.Any())
        {
            Loader.SetErrors(string.Join(Environment.NewLine, result.Errors.Select(x => x.ErrorMessage).ToArray()));
            
            return false;
        }

        return true;
    }

    private void StartLoading()
    {
        Loader.StartLoading();
    }

    private void StopLoading()
    {
        Loader.StopLoading();
    }

    private void SetLoadingErrors()
    {
        Loader.SetErrors(Errors.AuthenticationError);
    }
}
