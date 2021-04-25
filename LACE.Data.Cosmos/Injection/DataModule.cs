using System;
using LACE.Core.Abstractions.Configuration;
using LACE.Core.Abstractions.Injection;
using LACE.Core.Exceptions;
using LACE.Core.Extensions;
using LACE.Data.Cosmos.Abstractions;
using LACE.Data.Cosmos.Configuration;
using LACE.Data.Cosmos.Stores;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;

namespace LACE.Data.Cosmos.Injection
{
    public class DataModule : IModule
    {
        private readonly IConfigurationLoader _configurationLoader;

        public DataModule(IConfigurationLoader configurationLoader)
        {
            _configurationLoader = configurationLoader.Guard(nameof(configurationLoader));
        }
        
        public void Register(IServiceCollection services)
        {
            var dataConfiguration = _configurationLoader.Load<DataConfiguration>("Data");
            if (dataConfiguration == null)
            {
                throw new InjectionException();
            }

            var client = new CosmosClient(dataConfiguration.ConnectionString, new CosmosClientOptions
            {
                ApplicationName = dataConfiguration.ApplicationName
            });

            services.AddSingleton(client);
            services.AddSingleton(dataConfiguration);
            services.AddSingleton(dataConfiguration.Partitions);
            services.AddSingleton(dataConfiguration.Partitions.Default);
            services.AddScoped<CosmosDatabaseStore>();
            services.AddScoped<CosmosContainerStore>();
            services.AddScoped(typeof(IDocumentRepository<>), typeof(RepositoryPartition<>));
            services.AddScoped<IPartitionFactory, PartitionFactory>();
        }
    }
}
