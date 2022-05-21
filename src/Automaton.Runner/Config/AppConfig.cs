namespace Automaton.Runner.Core.Config
{
    public class AppConfig
    {
        public string RunnerName { get; set; }

        public bool IsRunnerRegistered() => !string.IsNullOrEmpty(RunnerName);
    }
}
