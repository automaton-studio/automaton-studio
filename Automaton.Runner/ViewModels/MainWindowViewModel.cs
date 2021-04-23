using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Automaton.Runner.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Properties

        private bool registrationVisible = false;
        public bool RegistrationVisible
        {
            get => registrationVisible;
            set
            {
                registrationVisible = value;
                OnPropertyChanged();
            }
        }

        private bool dashboardVisible = false;
        public bool DashboardVisible
        {
            get => dashboardVisible;
            set
            {
                dashboardVisible = value;
                OnPropertyChanged();
            }
        }

        private bool loginVisible = true;
        public bool LoginVisible
        {
            get => loginVisible;
            set
            {
                loginVisible = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public void ShowRegistrationControl()
        {
            LoginVisible = false;
            RegistrationVisible = true;
        }

        public void ShowDashboardControl()
        {
            LoginVisible = false;
            DashboardVisible = true;
        }

        // Create the OnPropertyChanged method to raise the event
        // The calling member's name will be used as the parameter.
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
