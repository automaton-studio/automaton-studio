using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Automaton.Runner.Pages.Setup;

partial class SetupPage : ComponentBase
{
    private Type setupStep = typeof(SetupDetails);

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
    }

    private void NavigateToSetupDetails()
    {
        setupStep = typeof(SetupDetails);
    }

    private void NavigateToInstallation()
    {
        setupStep = typeof(SetupDetails);
    }
}
