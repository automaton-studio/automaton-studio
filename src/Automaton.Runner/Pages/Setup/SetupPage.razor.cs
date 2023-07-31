using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Automaton.Runner.Pages.Setup;

partial class SetupPage : ComponentBase
{
    [Inject] private SetupViewModel SetupViewModel { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
    }
}
