using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using Automaton.Studio.Domain;

namespace Automaton.Studio.Pages.Designer.Components
{
    public class DropzoneStep : ComponentBase
    {
        [Parameter]
        public StudioStep Step { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (Step == null)
            {
                throw new ArgumentException("Invalid step parameter");
            }

            base.BuildRenderTree(builder);

            // get the component to view the product with
            Type componentType = Step.GetDesignerComponent();
            // create an instance of this component
            builder.OpenComponent(0, componentType);
            // set the `Step` attribute of the component
            builder.AddAttribute(1, "Step", Step);
            // close
            builder.CloseComponent();
        }
    }
}
