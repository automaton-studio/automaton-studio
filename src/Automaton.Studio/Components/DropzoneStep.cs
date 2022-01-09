using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using Automaton.Studio.Conductor;

namespace Automaton.Studio.Components
{
    /// <summary>
    /// A code-based component that renders a Studio Activity
    /// </summary>
    public class DropzoneStep : ComponentBase
    {
        /// <summary>
        /// The Activity we want to render
        /// </summary>
        [Parameter]
        public Step Step { get; set; }

        /// <summary>
        /// Render the component
        /// </summary>
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (Step == null)
            {
                throw new ArgumentException("Invalid Activity parameter");
            }

            base.BuildRenderTree(builder);

            // get the component to view the product with
            Type componentType = Step.GetDesignerComponent();
            // create an instance of this component
            builder.OpenComponent(0, componentType);
            // set the `Activity` attribute of the component
            builder.AddAttribute(1, "Activity", Step);
            // close
            builder.CloseComponent();
        }
    }
}
