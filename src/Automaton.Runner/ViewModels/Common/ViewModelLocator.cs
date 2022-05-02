using Microsoft.Extensions.DependencyInjection;

namespace Automaton.Runner.ViewModels
{
    /// <summary>
    /// Locator for all the ViewModels.
    /// </summary>
    /// <remarks>
    /// We use Locator to enable dependency injection into ViewModels.
    /// Without it, ViewModels would require a default constructor.
    /// </remarks>
    public class ViewModelLocator
    {
        public static LoginViewModel LoginViewModel
            => App.ServiceProvider.GetRequiredService<LoginViewModel>();

        public static MainWindowViewModel MainWindowViewModel
            => App.ServiceProvider.GetRequiredService<MainWindowViewModel>();

        public static RegistrationViewModel RegistrationViewModel
            => App.ServiceProvider.GetRequiredService<RegistrationViewModel>();

        public static DashboardViewModel DashboardViewModel
            => App.ServiceProvider.GetRequiredService<DashboardViewModel>();
    }
}
