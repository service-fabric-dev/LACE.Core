using LACE.Core.Exceptions;
using LACE.Core.Extensions;
using LACE.Data.Cosmos.Model;
using LACE.Data.Cosmos.UnitTests.Fixtures;
using LACE.Data.Cosmos.UnitTests.Model;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace LACE.Data.Cosmos.UnitTests
{
    [Collection(CosmosCollectionFixture.FixtureName)]
    public class DocumentRepositoryTests
    {
        private readonly CosmosFixtureData _fixture;
        private readonly CancellationToken _cancellationToken = CancellationToken.None;

        public DocumentRepositoryTests(CosmosFixtureData fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetAllAsync()
        {
            var containers             = _fixture.Containers;
            var dataConfiguration      = _fixture.DataConfig;
            var partitionConfiguration = dataConfiguration.Partitions.Values.First();

            var repo = new RepositoryPartition<StubDocument>(dataConfiguration, partitionConfiguration, containers);
            var documents = await repo.GetAllAsync(_cancellationToken);
            Assert.NotEmpty(documents);
        }

        [Fact]
        public async Task GetAsync_NoDocument_NotFoundException()
        {
            var containers             = _fixture.Containers;
            var dataConfiguration      = _fixture.DataConfig;
            var partitionConfiguration = dataConfiguration.Partitions.Values.First();

            var repo = new RepositoryPartition<StubDocument>(dataConfiguration, partitionConfiguration, containers);

            await Assert.ThrowsAsync<NotFoundException>(() => repo.GetAsync(Guid.NewGuid().ToString(), _cancellationToken));
        }

        [Fact]
        public async Task UpsertAsync_NullId_ArgumentNullException()
        {
            const string id = "test-id";
            var containers             = _fixture.Containers;
            var dataConfiguration      = _fixture.DataConfig;
            var partitionConfiguration = dataConfiguration.Partitions.Values.First();

            var document = new StubDocument
            {
                StubProperty = partitionConfiguration.PartitionKey
            };

            var repo = new RepositoryPartition<StubDocument>(dataConfiguration, partitionConfiguration, containers);

            await Assert.ThrowsAsync<ArgumentNullException>(() => repo.UpsertAsync(null, null, document, _cancellationToken));
            await Assert.ThrowsAsync<ArgumentNullException>(() => repo.UpsertAsync(string.Empty, null, document, _cancellationToken));
            await Assert.ThrowsAsync<ArgumentNullException>(() => repo.UpsertAsync(id, null, null, _cancellationToken));
        }

        [Fact]
        public async Task UpsertAsync_NewDocument_WrappedDocumentReturned()
        {
            const string id = "test-id";
            var document = new StubDocument
            {
                StubProperty = "stub-value"
            };

            var containers             = _fixture.Containers;
            var dataConfiguration      = _fixture.DataConfig;
            var partitionConfiguration = dataConfiguration.Partitions.Values.First();

            var repo = new RepositoryPartition<StubDocument>(dataConfiguration, partitionConfiguration, containers);

            var result = await repo.UpsertAsync(id, null, document, _cancellationToken);
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.False(result.Id.IsNullOrWhiteSpace());
            Assert.False(result.ETag.IsNullOrWhiteSpace());
            Assert.Equal(result.PartitionKey, partitionConfiguration.PartitionKey);
            Assert.True(result.State.HasFlag(DocumentState.Created | DocumentState.Updated));
            Assert.False(result.State.HasFlag(DocumentState.Unknown | DocumentState.Deleted | DocumentState.Faulted));
        }

        [Fact]
        public async Task UpsertAsync_ExistingDocument_ValuesUpdated()
        {
            const string id = "test-id";
            var newValue = Guid.NewGuid().ToString();

            var containers             = _fixture.Containers;
            var dataConfiguration      = _fixture.DataConfig;
            var partitionConfiguration = dataConfiguration.Partitions.Values.First();

            var repo     = new RepositoryPartition<StubDocument>(dataConfiguration, partitionConfiguration, containers);
            var document = await repo.GetAsync(id, _cancellationToken);

            document.Value.StubProperty = newValue;

            var result = await repo.UpsertAsync(id, document.ETag, document, _cancellationToken);
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.False(result.Id.IsNullOrWhiteSpace());
            Assert.False(result.ETag.IsNullOrWhiteSpace());
            Assert.Equal(result.PartitionKey, partitionConfiguration.PartitionKey);
            Assert.Equal(newValue, result.Value.StubProperty);
            Assert.True(result.State.HasFlag(DocumentState.Created | DocumentState.Updated));
            Assert.False(result.State.HasFlag(DocumentState.Unknown | DocumentState.Deleted | DocumentState.Faulted));
        }
    }
}