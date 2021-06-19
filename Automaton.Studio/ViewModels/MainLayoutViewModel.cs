using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Automaton.Studio.ViewModels
{
    public class MainLayoutViewModel : IMainLayoutViewModel, INotifyPropertyChanged
    {
        public MainLayoutViewModel()
        {
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
