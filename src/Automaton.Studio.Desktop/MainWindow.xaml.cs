using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace Automaton.Studio.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {            
            Resources.Add("services", App.ServiceCollection.BuildServiceProvider());

            InitializeComponent();
        }
    }

    // Workaround for compiler error "error MC3050: Cannot find the type 'local:Main'"
    // It seems that, although WPF's design-time build can see Razor components, its runtime build cannot.
    public partial class Main { }
}
