using Automaton.Runner.Resources;
using Automaton.Runner.Services;
using Automaton.Runner.Validators;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Automaton.Runner.ViewModels;

public class RegistrationViewModel
{
    private readonly RunnerService registrationService;
    private readonly RegistrationValidator registrationValidator;

    public LoaderViewModel Loader { get; set; }
    public string RunnerName { get; set; }

    public RegistrationViewModel
    (
        LoaderViewModel loader,
        RunnerService registrationService,
        RegistrationValidator registrationValidator
    )
    {
        this.Loader = loader;
        this.registrationService = registrationService;
        this.registrationValidator = registrationValidator;
    }

    public async Task<bool> Register()
    {
        try
        {
            if (!Validate())
            {
                SetLoadingErrors();
                return false;
            }

            StartLoading();

            await RegisterRunner();
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
        Loader.SetErrors(Errors.RegistrationError);
    }

    private async Task RegisterRunner()
    {
        await registrationService.Register(RunnerName);
    }
}
