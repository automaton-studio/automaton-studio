namespace Automaton.Runner.Config;

public class AppConfig
{
    public string ServerUrl { get; set; }
    public string RunnerName { get; set; }

    public bool RunnerRegistered => !string.IsNullOrEmpty(RunnerName);
}
