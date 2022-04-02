using AntDesign;
using Automaton.Studio.Components.NewFlow;
using Automaton.Studio.Models;
using Automaton.Studio.Services.Interfaces;
using Automaton.Studio.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages
{
    partial class LoginPage : ComponentBase
    {
        private bool loading = false;

        [Inject] private NavigationManager NavigationManager { get; set; } = default!;
        [Inject] private ILoginViewModel LoginViewModel { get; set; } = default!;
        [Inject] public INavMenuService NavMenuService { get; set; }

        public LoginModel Model => LoginViewModel.Model;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }

        private void OnFinish(EditContext editContext)
        {
            NavigationManager.NavigateTo($"flows");
        }

        private void OnFinishFailed(EditContext editContext)
        {
            
        }
    }
}
