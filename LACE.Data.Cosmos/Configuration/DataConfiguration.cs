namespace LACE.Data.Cosmos.Configuration
{
    public class DataConfiguration
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string ApplicationName { get; set; }
        public RepositoryPartitionConfigurations Partitions { get; set; }
    }
}
