using LACE.Data.Cosmos.Configuration;
using Microsoft.Azure.Cosmos;
using System;
using LACE.Data.Cosmos.Stores;
using Microsoft.Azure.Cosmos.Fluent;
using Xunit;

namespace LACE.Data.Cosmos.UnitTests.Fixtures
{
    public class CosmosFixtureData : IDisposable
    {
        private const string ConnectionString = "AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";

        public const string DatabaseName     = "lace-data-cosmos-unittests";
        public const string ContainerName    = "test-collection";
        public const string PartitionKey     = "test-partition";
        public const string PartitionKeyPath = "/PartitionKey";

        public DataConfiguration                DataConfig      { get; } = InitializeConfig();
        public RepositoryPartitionConfiguration PartitionConfig { get; } = InitializePartition();
        public CosmosClient                     Client          { get; } = InitializeClient();
        public CosmosDatabaseStore              Databases       => new(Client);
        public CosmosContainerStore             Containers      => new(Databases);
        public PartitionKey                     TestPartition   => new(PartitionConfig.PartitionKey);

        public void Dispose()
        {
            Client?.Dispose();
        }

        private static DataConfiguration InitializeConfig()
        {
            return new()
            {
                ConnectionString = ConnectionString,
                DatabaseName     = DatabaseName
            };
        }

        private static RepositoryPartitionConfiguration InitializePartition()
        {
            return new()
            {
                ContainerName = ContainerName,
                PartitionKey = PartitionKey,
                PartitionKeyPath = PartitionKeyPath
            };
        }

        private static CosmosClient InitializeClient()
        {
            return new(ConnectionString, new CosmosClientOptions
            {
                ApplicationName = "UnitTests"
            });
        }

        private CosmosDatabaseStore InitializeDatabaseStore()
        {
            return new(Client);
        }

        private CosmosContainerStore InitializeContainerStore()
        {
            return new(Databases);
        }
    }

    [CollectionDefinition(FixtureName)]
    public class CosmosCollectionFixture : ICollectionFixture<CosmosFixtureData>
    {
        public const string FixtureName = "CosmosCollectionFixture";

        //    private readonly CosmosFixtureData _fixtureData;

        //    public CosmosCollectionFixture(CosmosFixtureData fixtureData)
        //    {
        //        _fixtureData = fixtureData;
        //    }
        //}
    }
}