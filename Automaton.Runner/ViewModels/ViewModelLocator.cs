using Microsoft.Extensions.DependencyInjection;

namespace Automaton.Runner.ViewModels
{
    /// <summary>
    /// Locator for all the ViewModels
    /// </summary>
    public class ViewModelLocator
    {
        public static LoginViewModel LoginViewModel
            => App.ServiceProvider.GetRequiredService<LoginViewModel>();
    }
}
