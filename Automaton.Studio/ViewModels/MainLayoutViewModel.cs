using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Automaton.Studio.ViewModels
{
    public class MainLayoutViewModel : IMainLayoutViewModel, INotifyPropertyChanged
    {
        #region Events

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
