namespace Automaton.Runner.Core.Config
{
    public class UserConfig
    {
        public string RunnerName { get; set; }

        public bool IsRunnerRegistered() => !string.IsNullOrEmpty(RunnerName);
    }
}
