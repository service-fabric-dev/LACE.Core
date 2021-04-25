using System.Linq;
using LACE.Core.Abstractions.Model;
using LACE.Core.Exceptions;
using LACE.Core.Extensions;
using LACE.Data.Cosmos.Abstractions;
using LACE.Data.Cosmos.Configuration;
using LACE.Data.Cosmos.Stores;

namespace LACE.Data.Cosmos
{
    public class PartitionFactory : IPartitionFactory
    {
        private readonly DataConfiguration _dataConfiguration;
        private readonly RepositoryPartitionConfigurations _repositoryConfigurations;
        private readonly CosmosContainerStore _containerStore;

        public PartitionFactory(
            DataConfiguration dataConfiguration,
            CosmosContainerStore containerStore)
        {
            _dataConfiguration        = dataConfiguration.Guard(nameof(dataConfiguration));
            _repositoryConfigurations = dataConfiguration.Partitions.Guard(nameof(dataConfiguration.Partitions));
            _containerStore           = containerStore.Guard(nameof(containerStore));
        }

        private RepositoryPartitionConfiguration DefaultRepoConfiguration => _repositoryConfigurations.Default ?? _repositoryConfigurations.Values.FirstOrDefault() ?? throw new InternalServerErrorException("No repositories have been configured");

        private IDocumentRepository<TType> DefaultRepo<TType>() => new RepositoryPartition<TType>(
            _dataConfiguration,
            DefaultRepoConfiguration,
            _containerStore);

        public IDocumentRepository<TType> Build<TType>(string name)
        {
            name.Guard(nameof(name));
            if (_dataConfiguration.Partitions?.ContainsKey(name) != true)
            {
                return DefaultRepo<TType>();
            }

            return new RepositoryPartition<TType>(
                _dataConfiguration,
                _dataConfiguration.Partitions[name],
                _containerStore);
        }
    }
}
