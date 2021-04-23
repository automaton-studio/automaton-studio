using Automaton.Runner.ViewModels.Common;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Automaton.Runner.ViewModels
{
    public class ViewModelLoader : IViewModelLoader, INotifyPropertyChanged
    {
        #region Properties

        private string errors;
        public string Errors
        {
            get => errors;
            set
            {
                errors = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasErrors));
            }
        }

        public bool HasErrors
        {
            get => !string.IsNullOrEmpty(Errors);
        }

        private bool loading;
        public bool Loading
        {
            get => loading;
            set
            {
                loading = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Methods

        public void SetErrors(string errors)
        {
            Errors = errors;
        }

        public void ClearErrors()
        {
            Errors = string.Empty;
        }

        public void StartLoading()
        {
            Loading = true;
        }

        public void StopLoading()
        {
            Loading = false;
        }

        #endregion


        public event PropertyChangedEventHandler PropertyChanged;

        // Create the OnPropertyChanged method to raise the event
        // The calling member's name will be used as the parameter.
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
