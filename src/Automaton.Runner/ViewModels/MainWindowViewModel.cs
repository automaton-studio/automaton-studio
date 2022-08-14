using Automaton.Runner.Core.Services;
using Automaton.Runner.Services;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Automaton.Runner.ViewModels;

public class MainWindowViewModel : INotifyPropertyChanged
{
    private readonly HubService hubService;
    private readonly ConfigService configService;
    private readonly AuthenticationService authenticationService;
    private readonly ILogger<MainWindowViewModel> logger;

    private bool loginVisible;
    public bool LoginVisible
    {
        get
        {
            return loginVisible;
        }
        set
        {
            loginVisible = value;
            OnPropertyChanged();
        }
    }

    private bool homeVisible;
    public bool HomeVisible
    {
        get
        {
            return homeVisible;
        }
        set
        {
            homeVisible = value;
            OnPropertyChanged();
        }
    }

    private bool registerVisible;
    public bool RegisterVisible
    {
        get
        {
            return registerVisible;
        }
        set
        {
            registerVisible = value;
            OnPropertyChanged();
        }
    }

    public MainWindowViewModel(HubService hubService, 
        ConfigService configService, 
        AuthenticationService authenticationService,
        ILogger<MainWindowViewModel> logger)
    {
        this.hubService = hubService;
        this.configService = configService;
        this.authenticationService = authenticationService;
        this.logger = logger;
    }

    public void InitializeNavigation()
    {
        LoginVisible = !IsAuthenticated();
        RegisterVisible = !IsRunnerRegistered();
        HomeVisible = !LoginVisible && !RegisterVisible;
    }

    public bool IsAuthenticated()
    {
        var authenticated = authenticationService.IsAuthenticated();

        return authenticated;
    }

    public bool IsRunnerRegistered()
    {
        var registered = configService.AppConfig.IsRunnerRegistered();

        return registered;
    }

    public async Task Logout()
    {
       await authenticationService.Logout();
    }

    public async Task Disconnect()
    {
        await hubService.Disconnect();
    }

    public void ApplyLoginMenuVisibility()
    {
        HomeVisible = false;
        LoginVisible = true;
        RegisterVisible = false;
    }

    public void ApplyRegistrationMenuVisibility()
    {
        HomeVisible = false;
        LoginVisible = false;
        RegisterVisible = true;
    }

    public void ApplyHomeMenuVisibility()
    {
        HomeVisible = true;
        LoginVisible = false;
        RegisterVisible = false;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    // Create the OnPropertyChanged method to raise the event
    // The calling member's name will be used as the parameter.
    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
