using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;

namespace Automaton.Studio.Components
{
    public class ServerSideValidator : ComponentBase
    {
        private ValidationMessageStore _messageStore;
        private Dictionary<string, List<string>> _errors { get; set; } = new Dictionary<string, List<string>>();

        [CascadingParameter] EditContext CurrentEditContext { get; set; }

        protected override void OnInitialized()
        {
            if (CurrentEditContext == null)
            {
                throw new InvalidOperationException($"{nameof(ServerSideValidator)} requires a cascading " +
                    $"parameter of type {nameof(EditContext)}. For example, you can use {nameof(ServerSideValidator)} " +
                    $"inside an {nameof(EditForm)}.");
            }

            _messageStore = new ValidationMessageStore(CurrentEditContext);

            CurrentEditContext.OnValidationRequested += (s, e) => _messageStore.Clear();
            CurrentEditContext.OnFieldChanged += (s, e) => _messageStore.Clear(e.FieldIdentifier);
        }

        public void AddError(string key, string error)
        {
            if (!_errors.ContainsKey(key))
            {
                _errors.Add(key, new List<string> { error });
            }
        }

        public void DisplayErrors()
        {
            foreach (var error in _errors)
            {
                _messageStore.Add(CurrentEditContext.Field(error.Key), error.Value);
            }

            CurrentEditContext.NotifyValidationStateChanged();
        }

        public void ClearErrors()
        {
            _errors.Clear();
            _messageStore.Clear();
        }
    }
}
