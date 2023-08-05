namespace Automaton.Runner.Storage;

public class ApplicationStorage
{
    public void SetRunnerName(string runnerName)
    {
        Properties.Settings.Default.RunnerName = runnerName;
        Properties.Settings.Default.Save();
    }

    public string GetRunnerName()
    {
        return Properties.Settings.Default.RunnerName;
    }

    public void SetServerUrl(string serverUrl)
    {
        Properties.Settings.Default.ServerUrl = serverUrl;
        Properties.Settings.Default.Save();
    }

    public string GetServerUrl()
    {
        return Properties.Settings.Default.ServerUrl;
    }

    public bool IsRunnerRegistered() => !string.IsNullOrEmpty(GetRunnerName());
    public bool IsServerRegistered() => !string.IsNullOrEmpty(GetServerUrl());
}
