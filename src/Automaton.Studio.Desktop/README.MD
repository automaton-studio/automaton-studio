# Automaton.Desktop documentation


## TIPS

Just saving some development tips in the README file.

### Mixed Blazor and WPF controls

In MainWindow.xaml we can have:

``
<StackPanel Orientation="Horizontal" DockPanel.Dock="Top" Background="CadetBlue">
    <Label>This is native WPF UI</Label>
    <Button Click="Button_Click" Margin="4" Padding="4">Check counter</Button>
 </StackPanel>
``

And in MainWondow.xaml.cs

``
private void Button_Click(object sender, RoutedEventArgs e)
{
    MessageBox.Show(
        owner: this,
        messageBoxText: $"Current counter value is: {App.AppState.Counter}",
        caption: "Counter");
}
``