﻿using Automaton.Studio.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Threading.Tasks;

namespace Automaton.Studio.Components
{
    partial class StepExplorer : ComponentBase
    {
        [Inject] 
        private IStepsViewModel StepsViewModel { get; set; } = default!;

        private string searchText { get; set; }

        protected override async Task OnInitializedAsync()
        {
            StepsViewModel.Initialize();

            await base.OnInitializedAsync();
        }

        private void OnSearchChange(string text)
        {
            throw new NotImplementedException();
        }

        public async Task OnSearch()
        {
            throw new NotImplementedException();
        }

        private async Task OnEnter(KeyboardEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}