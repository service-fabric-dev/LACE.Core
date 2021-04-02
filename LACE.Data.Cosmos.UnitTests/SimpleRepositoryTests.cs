using System;
using System.Threading;
using System.Threading.Tasks;
using LACE.Core.Exceptions;
using LACE.Core.Extensions;
using LACE.Data.Cosmos.Configuration;
using LACE.Data.Cosmos.Enums;
using LACE.Data.Cosmos.UnitTests.Fixtures;
using LACE.Data.Cosmos.UnitTests.Model;
using Xunit;

namespace LACE.Data.Cosmos.UnitTests
{
    [Collection(CosmosCollectionFixture.FixtureName)]
    public class SimpleRepositoryTests
    {
        private readonly CosmosFixtureData _fixture;
        private readonly CancellationToken _cancellationToken = CancellationToken.None;

        public SimpleRepositoryTests(CosmosFixtureData fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetAsync_NoDocument_NotFoundException()
        {
            var containers    = _fixture.Containers;
            var configuration = _fixture.DataConfig;

            var repo = new SimpleRepository<StubDocument>(containers, configuration);

            await Assert.ThrowsAsync<NotFoundException>(() => repo.GetAsync(Guid.NewGuid().ToString(), _cancellationToken));
        }

        [Fact]
        public async Task UpsertAsync_NullId_ArgumentNullException()
        {
            const string id = "test-id";
            var document = new StubDocument
            {
                StubProperty = _fixture.DataConfig.PartitionKey
            };

            var containers    = _fixture.Containers;
            var configuration = _fixture.DataConfig;

            var repo = new SimpleRepository<StubDocument>(containers, configuration);

            await Assert.ThrowsAsync<ArgumentNullException>(() => repo.UpsertAsync(null,         document, _cancellationToken));
            await Assert.ThrowsAsync<ArgumentNullException>(() => repo.UpsertAsync(string.Empty, document, _cancellationToken));
            await Assert.ThrowsAsync<ArgumentNullException>(() => repo.UpsertAsync(id, null, _cancellationToken));
        }

        [Fact]
        public async Task UpsertAsync_ValidDocument_WrappedDocumentReturned()
        {
            const string id = "test-id";
            var document = new StubDocument
            {
                StubProperty = "stub-value"
            };

            var containers    = _fixture.Containers;
            var configuration = _fixture.DataConfig;

            var repo = new SimpleRepository<StubDocument>(containers, configuration);

            var result = await repo.UpsertAsync(id, document, _cancellationToken);
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.False(result.Id.IsNullOrWhiteSpace());
            Assert.False(result.ETag.IsNullOrWhiteSpace());
            Assert.Equal(result.PartitionKey, _fixture.DataConfig.PartitionKey);
            Assert.True(result.State.HasFlag(DocumentState.Created  | DocumentState.Updated));
            Assert.False(result.State.HasFlag(DocumentState.Unknown | DocumentState.Deleted | DocumentState.Faulted));
        }
    }
}