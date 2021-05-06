using Automaton.Studio.Components.ActionBar;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Automaton.Studio.ViewModels
{
    public class MainLayoutViewModel : IMainLayoutViewModel, INotifyPropertyChanged
    {
        #region Properties

        private ActionBarComponent? actionBar;
        public ActionBarComponent ActionBar
        {
            get => actionBar;

            set
            {
                actionBar = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        #endregion

        public MainLayoutViewModel()
        {
        }

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
