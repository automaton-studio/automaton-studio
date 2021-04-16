using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Automaton.Runner.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool setupVisible = false;
        public bool SetupVisible
        {
            get { return setupVisible; }
            set
            {
                setupVisible = value;
                OnPropertyChanged();
            }
        }

        private bool loginVisible = true;
        public bool LoginVisible
        {
            get { return loginVisible; }
            set
            {
                loginVisible = value;
                OnPropertyChanged();
            }
        }

        public void ShowSetup()
        {
            LoginVisible = false;
            SetupVisible = true;
        }

        // Create the OnPropertyChanged method to raise the event
        // The calling member's name will be used as the parameter.
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
