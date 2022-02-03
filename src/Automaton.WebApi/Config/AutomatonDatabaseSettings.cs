namespace Automaton.WebApi.Config
{
    public class AutomatonDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string FlowsCollectionName { get; set; } = null!;
    }
}
