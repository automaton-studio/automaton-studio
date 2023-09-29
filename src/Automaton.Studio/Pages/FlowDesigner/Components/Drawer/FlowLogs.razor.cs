using Automaton.Core.Logs;
using Automaton.Studio.Domain;
using Microsoft.AspNetCore.Components;

namespace Automaton.Studio.Pages.FlowDesigner.Components.Drawer;

public partial class FlowLogs : ComponentBase
{
    [CascadingParameter]
    private StudioFlow Flow { get; set; }

    [Inject]
    public WorkflowLogsSink WorkflowSink { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
    }

    private void ClearLogs()
    {
        WorkflowSink.ClearLogs();
    }
}
