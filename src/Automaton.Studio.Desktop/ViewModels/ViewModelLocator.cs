using Automaton.Studio.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Automaton.Studio.Desktop.ViewModels
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
        public static DefinitionsViewModel DefinitionsViewModel
            => App.ServiceProvider.GetRequiredService<DefinitionsViewModel>();
    }
}
