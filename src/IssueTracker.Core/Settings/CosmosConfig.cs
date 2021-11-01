namespace IssueTracker.Core.Settings
{
    public interface ICosmosConfig
    {
        string ConnectionString { get; }
        string DatabaseName { get; }
    }

    public class CosmosConfig : ICosmosConfig
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;
    }
}
