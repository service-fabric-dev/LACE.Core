using System.Net;
using System.Threading.Tasks;
using LACE.Core.Exceptions;
using LACE.Core.Extensions;
using Microsoft.Azure.Cosmos;

namespace LACE.Data.Cosmos.Stores
{
    public class CosmosContainerStore
    {
        private readonly CosmosDatabaseStore _databaseStore;

        public CosmosContainerStore(CosmosDatabaseStore databaseStore)
        {
            _databaseStore = databaseStore.Guard(nameof(databaseStore));
        }

        public async Task<Container> GetAsync(string databaseName, string containerName, string partitionKeyPath)
        {
            if (databaseName.IsNullOrWhiteSpace() || containerName.IsNullOrWhiteSpace())
            {
                return null;
            }

            var database = await _databaseStore
                .GetAsync(databaseName)
                .ConfigureAwait(false);
            if (database == null)
            {
                return null;
            }

            var containerProperties = new ContainerProperties(containerName, partitionKeyPath);
            var response = await database
                .CreateContainerIfNotExistsAsync(containerProperties)
                .ConfigureAwait(false);
            return response.StatusCode switch
            {
                HttpStatusCode.OK => response,
                HttpStatusCode.Created => response,
                _ => throw new InternalServerErrorException($"Failed to get container {containerName} from database {databaseName}")
            };
        }
    }
}
