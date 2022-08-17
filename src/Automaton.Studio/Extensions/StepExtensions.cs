using AntDesign;
using System;
using System.Threading.Tasks;

namespace Automaton.Studio.Extensions;

public static class StepExtensions
{
    public static Task<ModalRef> CreateDynamicModalAsync<TComponent, TComponentOptions>(this ModalService modalService, ModalOptions config, TComponentOptions componentOptions) where TComponent : FeedbackComponent<TComponentOptions>
    {
       return modalService.CreateModalAsync<TComponent, TComponentOptions>(config, componentOptions);  
    }

    public static async Task<ModalRef> DisplayPropertiesDialog(this Domain.StudioStep step, ModalService modalService)
    {
        var modalConfig = new ModalOptions
        {
            Title = step.DisplayName
        };

        // 1. Select the method to be executed
        var method = typeof(StepExtensions).GetMethod(nameof(CreateDynamicModalAsync));
        // 2. Make the metod generic because CreateDynamicModalAsync is using generics
        var generic = method.MakeGenericMethod(step.GetPropertiesComponent(), step.GetType());
        // 3. Invoke the method and pass the required parameters
        var result = await generic.InvokeAsync(modalService, new object[] { modalService, modalConfig, step }) as ModalRef;

        return result;
    }
}
