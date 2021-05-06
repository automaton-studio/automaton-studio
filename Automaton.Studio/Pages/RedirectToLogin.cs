using Microsoft.AspNetCore.Components;
using System;

namespace Automaton.Studio.Pages
{
    public class RedirectToLogin : ComponentBase
    {
        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            NavigationManager.NavigateTo($"Identity/Account/Login");
        }
    }
}
