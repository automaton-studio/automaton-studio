using AntDesign;
using System.Threading.Tasks;

namespace Automaton.Studio.Core.Extensions
{
    public static class ActivityExtensions
    {
        public static async Task<ModalRef> EditActivityDialog(this StudioActivity activity, ModalService modalService)
        {
            var modalConfig = new ModalOptions
            {
                Title = activity.Descriptor.DisplayName
            };

            // Launch the Properties dialog using reflection to dynamically load the activity properties component.

            // 1. Select the method to be executed
            var method = typeof(ModalService).GetMethod(nameof(modalService.CreateDynamicModalAsync));
            // 2. Make the metod generic because CreateDynamicModalAsync is using generics
            var generic = method.MakeGenericMethod(activity.GetPropertiesComponent(), activity.GetType());
            // 3. Invoke the method and pass the required parameters
            var result = await generic.InvokeAsync(modalService, new object[] { modalConfig, activity }) as ModalRef;

            return result;
        } 
    }
}
