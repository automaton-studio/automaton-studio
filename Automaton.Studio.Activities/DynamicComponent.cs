using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;

namespace Automaton.Studio.Activities
{
    /// <summary>
    /// A code-based component that renders a product using its GetViewComponent result
    /// </summary>
    public class DynamicComponent : ComponentBase
    {
        /// <summary>
        /// The product we want to render
        /// </summary>
        [Parameter]
        public ActivityBase Activity { get; set; }

        /// <summary>
        /// Render the component
        /// </summary>
        /// <param name="builder"></param>
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            // get the component to view the product with
            Type componentType = Activity.GetViewComponent();
            // create an instance of this component
            builder.OpenComponent(0, componentType);
            // set the `Product` attribute of the component
            builder.AddAttribute(1, "Activity", Activity);
            // close
            builder.CloseComponent();
        }
    }
}
