namespace LACE.Data.Cosmos.Configuration
{
    public class RepositoryPartitionConfiguration
    {
        public string ContainerName    { get; set; }
        public string PartitionKey     { get; set; }
        public string PartitionKeyPath { get; set; }
    }
}
