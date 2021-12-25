// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Automaton.Studio.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                owner: this,
                messageBoxText: $"Current counter value is: {App.AppState.Counter}",
                caption: "Counter");
        }
    }

    // Workaround for compiler error "error MC3050: Cannot find the type 'local:Main'"
    // It seems that, although WPF's design-time build can see Razor components, its runtime build cannot.
    public partial class Main { }
}
