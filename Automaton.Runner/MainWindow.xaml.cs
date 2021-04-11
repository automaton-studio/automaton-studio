using Automaton.Runner.Core;
using System.Windows;
using Microsoft.AspNetCore.SignalR.Client;
using System.Threading.Tasks;
using System;
using System.Windows.Input;
using System.Diagnostics;

namespace Automaton.Runner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private HubConnection connection;
        private readonly IWorkflowManager workflowManager;

        public MainWindow(IWorkflowManager workflowManager)
        {
            InitializeComponent();

            loginControl.LoginSuccessful += LoginSuccessful;

            this.workflowManager = workflowManager;   
        }

        private async void LoginSuccessful(object sender, AuthTokenArgs e)
        {
            try
            {

                connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:5001/WorkflowHub", options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(e.AuthToken.AccessToken);
                })
                .Build();

                connection.Closed += async (error) =>
                {
                    await Task.Delay(new Random().Next(0, 5) * 1000);
                    await connection.StartAsync();
                };

                connection.On<string>("RunWorkflow", (definitionId) =>
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        Trace.WriteLine($"Running workflow definition {definitionId}");
                        this.workflowManager.RunWorkflow(definitionId);
                    });
                });

                connection.On<string>("WelcomeRunner", (name) =>
                {
                });

                await connection.StartAsync();
            }
            catch (Exception ex)
            {
            }
        }

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
    }
}
