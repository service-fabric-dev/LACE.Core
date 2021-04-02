using LACE.Core.Exceptions;
using LACE.Core.Extensions;
using LACE.Data.Cosmos.Model;
using LACE.Data.Cosmos.UnitTests.Fixtures;
using LACE.Data.Cosmos.UnitTests.Model;
using System;
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
        public async Task GetAsync_NoDocument_NotFoundException()
        {
            var containers    = _fixture.Containers;
            var configuration = _fixture.DataConfig;

            var repo = new DocumentRepository<StubDocument>(containers, configuration);

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

            var repo = new DocumentRepository<StubDocument>(containers, configuration);

            await Assert.ThrowsAsync<ArgumentNullException>(() => repo.UpsertAsync(null,         null, document, _cancellationToken));
            await Assert.ThrowsAsync<ArgumentNullException>(() => repo.UpsertAsync(string.Empty, null, document, _cancellationToken));
            await Assert.ThrowsAsync<ArgumentNullException>(() => repo.UpsertAsync(id,           null, null,     _cancellationToken));
        }

        [Fact]
        public async Task UpsertAsync_NewDocument_WrappedDocumentReturned()
        {
            const string id = "test-id";
            var document = new StubDocument
            {
                StubProperty = "stub-value"
            };

            var containers    = _fixture.Containers;
            var configuration = _fixture.DataConfig;

            var repo = new DocumentRepository<StubDocument>(containers, configuration);

            var result = await repo.UpsertAsync(id, null, document, _cancellationToken);
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.False(result.Id.IsNullOrWhiteSpace());
            Assert.False(result.ETag.IsNullOrWhiteSpace());
            Assert.Equal(result.PartitionKey, _fixture.DataConfig.PartitionKey);
            Assert.True(result.State.HasFlag(DocumentState.Created  | DocumentState.Updated));
            Assert.False(result.State.HasFlag(DocumentState.Unknown | DocumentState.Deleted | DocumentState.Faulted));
        }

        [Fact]
        public async Task UpsertAsync_ExistingDocument_ValuesUpdated()
        {
            const string id       = "test-id";
            var          newValue = Guid.NewGuid().ToString();

            var containers    = _fixture.Containers;
            var configuration = _fixture.DataConfig;

            var repo     = new DocumentRepository<StubDocument>(containers, configuration);
            var document = await repo.GetAsync(id, _cancellationToken);

            document.Value.StubProperty = newValue;

            var result = await repo.UpsertAsync(id, document.ETag, document, _cancellationToken);
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.False(result.Id.IsNullOrWhiteSpace());
            Assert.False(result.ETag.IsNullOrWhiteSpace());
            Assert.Equal(result.PartitionKey, _fixture.DataConfig.PartitionKey);
            Assert.Equal(newValue, result.Value.StubProperty);
            Assert.True(result.State.HasFlag(DocumentState.Created  | DocumentState.Updated));
            Assert.False(result.State.HasFlag(DocumentState.Unknown | DocumentState.Deleted | DocumentState.Faulted));
        }
    }
}