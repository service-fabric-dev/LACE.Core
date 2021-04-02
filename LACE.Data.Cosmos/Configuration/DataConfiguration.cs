using LACE.Core.Abstractions.Configuration;

namespace LACE.Data.Cosmos.Configuration
{
    public class DataConfiguration : IConfiguration
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string ContainerName { get; set; }
        public string PartitionKeyPath { get; set; }
        public string PartitionKey { get; set; }
    }

    public class StorageEndpoint
    {
        public DataConfiguration Configuration { get; set; }
    }
}
