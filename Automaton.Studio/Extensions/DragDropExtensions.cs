using Automaton.Studio.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Automaton.Studio.DragDrop
{
    public static class DragDropExtensions
    {
        public static IServiceCollection AddBlazorDragDrop(this IServiceCollection services)
        {
            return services.AddScoped(typeof(DragDropService<>));
        }
    }
}
