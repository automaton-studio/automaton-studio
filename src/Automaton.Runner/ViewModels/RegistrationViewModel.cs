using Automaton.Runner.Enums;
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
    public bool HasErrors => Loader.HasErrors;

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
