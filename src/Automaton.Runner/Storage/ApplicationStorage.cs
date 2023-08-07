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

    public void SetRunnerId(string id)
    {
        Properties.Settings.Default.RunnerId = id;
        Properties.Settings.Default.Save();
    }

    public string GetRunnerId()
    {
        return Properties.Settings.Default.RunnerId;
    }

    public bool IsRunnerRegistered() => !string.IsNullOrEmpty(GetRunnerId());
}
