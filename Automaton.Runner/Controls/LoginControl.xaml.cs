using System.Windows;
using System.Windows.Controls;

namespace Automaton.Runner.Controls
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class LoginControl : UserControl
    {
        public static readonly RoutedEvent OnLoginSuccessfulHandler = EventManager.RegisterRoutedEvent("LoginSuccessful", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(LoginControl));

        public event RoutedEventHandler OnLoginSuccessful
        {
            add { AddHandler(OnLoginSuccessfulHandler, value); }
            remove { RemoveHandler(OnLoginSuccessfulHandler, value); }
        }

        public LoginControl()
        {
            InitializeComponent();
        }

        private void LoginSuccessful(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(OnLoginSuccessfulHandler));
        }

        private void LoginClick(object sender, RoutedEventArgs e)
        {
            var username = UsernameBox.Text;
            var password = PasswordBox.Password;
        }
    }
}
