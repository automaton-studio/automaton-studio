using Automaton.Runner.Core;
using System.Windows;

namespace Automaton.Runner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IWorkflowManager workflowManager;

        public MainWindow(IWorkflowManager workflowManager)
        {
            InitializeComponent();

            this.workflowManager = workflowManager;

            this.workflowManager.RunWorkflow("HelloWorldWorkflow");
        }
    }
}
