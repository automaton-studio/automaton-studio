using AntDesign;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Automaton.Studio.Extensions
{
    public static class StepExtensions
    {
        /// <summary>
        /// Create and open a Modal with template
        /// </summary>
        /// <typeparam name="TComponent"></typeparam>
        /// <typeparam name="TComponentOptions"></typeparam>
        /// <param name="config"></param>
        /// <param name="componentOptions"></param>
        /// <returns></returns>
        public static Task<ModalRef> CreateDynamicModalAsync<TComponent, TComponentOptions>(this ModalService modalService, ModalOptions config, TComponentOptions componentOptions) where TComponent : FeedbackComponent<TComponentOptions>
        {
           return modalService.CreateModalAsync<TComponent, TComponentOptions>(config, componentOptions);  
        }

        public static async Task<ModalRef> EditStepDialog(this Conductor.Step step, ModalService modalService)
        {
            var modalConfig = new ModalOptions
            {
                Title = step.Name
            };

            // Launch the Properties dialog using reflection to dynamically load the activity properties component.

            // 1. Select the method to be executed
            var method = typeof(StepExtensions).GetMethod(nameof(CreateDynamicModalAsync));
            // 2. Make the metod generic because CreateDynamicModalAsync is using generics
            var generic = method.MakeGenericMethod(step.GetPropertiesComponent(), step.GetType());
            // 3. Invoke the method and pass the required parameters
            var result = await generic.InvokeAsync(modalService, new object[] { modalService, modalConfig, step }) as ModalRef;

            return result;
        }
    }
}
