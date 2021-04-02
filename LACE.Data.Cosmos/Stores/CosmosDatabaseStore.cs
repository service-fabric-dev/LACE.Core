using System.Net;
using System.Threading.Tasks;
using LACE.Core.Exceptions;
using LACE.Core.Extensions;
using Microsoft.Azure.Cosmos;

namespace LACE.Data.Cosmos.Stores
{
    public class CosmosDatabaseStore
    {
        private readonly CosmosClient _client;

        public CosmosDatabaseStore(CosmosClient client)
        {
            _client = client.Guard(nameof(client));
        }

        public async Task<Database> GetAsync(string databaseName)
        {
            databaseName.GuardNullOrWhiteSpace(nameof(databaseName));

            var response = await _client.CreateDatabaseIfNotExistsAsync(databaseName);
            if (response.Database == null)
            {
                throw new InternalServerErrorException($"{nameof(CosmosDatabaseStore)}, database {databaseName}: Failed to create database");
            }

            return response.StatusCode == HttpStatusCode.OK ? (Database)response : null;
        }
    }
}
