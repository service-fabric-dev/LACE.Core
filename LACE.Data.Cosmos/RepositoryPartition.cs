using LACE.Core.Exceptions;
using LACE.Core.Extensions;
using LACE.Data.Cosmos.Abstractions;
using LACE.Data.Cosmos.Configuration;
using LACE.Data.Cosmos.Exceptions;
using LACE.Data.Cosmos.Model;
using LACE.Data.Cosmos.Stores;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace LACE.Data.Cosmos
{
    public class RepositoryPartition<TDocument> : IDocumentRepository<TDocument>
    {
        private readonly DataConfiguration _dataConfiguration;
        private readonly RepositoryPartitionConfiguration _partitionConfiguration;
        private readonly CosmosContainerStore _containerStore;
        private readonly string _partitionKeyValue;
        private readonly PartitionKey _partitionKey;

        public RepositoryPartition(
            DataConfiguration dataConfiguration,
            RepositoryPartitionConfiguration partitionConfiguration,
            CosmosContainerStore containerStore)
        {
            _dataConfiguration = dataConfiguration.Guard(nameof(dataConfiguration));
            _partitionConfiguration = partitionConfiguration.Guard(nameof(partitionConfiguration));
            _containerStore    = containerStore.Guard(nameof(containerStore));
            _partitionKeyValue = partitionConfiguration.PartitionKey.Guard(nameof(partitionConfiguration.PartitionKey));
            _partitionKey      = new PartitionKey(_partitionKeyValue);
        }

        private Task<Container> GetContainerAsync() => _containerStore.GetAsync(
            _dataConfiguration.DatabaseName,
            _partitionConfiguration.ContainerName,
            _partitionConfiguration.PartitionKeyPath);

        public async Task<List<DocumentContainer<TDocument>>> GetAllAsync(CancellationToken cancellationToken)
        {
            var documentContainers = new List<DocumentContainer<TDocument>>();

            var container = await GetContainerAsync();
            using var iterator = container.GetItemQueryIterator<DocumentContainer<TDocument>>(
                requestOptions: new QueryRequestOptions
                {
                    PartitionKey = _partitionKey
                });
            while (iterator.HasMoreResults)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var feedResponse = await TryAsync(() => iterator.ReadNextAsync(cancellationToken));

                var validationExceptions = new List<Exception>();
                foreach (var entry in feedResponse) // Implicit cast here is so slick
                {
                    switch (ValidateDocument(nameof(GetAsync), entry.Id, entry, out var validated))
                    {
                        case null:
                            documentContainers.Add(entry);
                            break;
                        case { } exception:
                            validationExceptions.Add(exception);
                            break;
                    }
                }

                switch (validationExceptions.Count)
                {
                    case 1:
                        throw validationExceptions[0];
                    case { } count when count > 1:
                        throw new AggregateException(validationExceptions);
                }

            }

            return documentContainers;
        }

        public async Task<DocumentContainer<TDocument>> GetAsync(string id, CancellationToken cancellationToken)
        {
            id.GuardNullOrWhiteSpace(nameof(id));

            var container = await GetContainerAsync();
            var response = await TryAsync(() => container.ReadItemAsync<DocumentContainer<TDocument>>(id, _partitionKey, cancellationToken: cancellationToken));
            return ValidateDocument(nameof(GetAsync), id, response.Resource, out var validated) switch
            {
                null => validated,
                { } e => throw e
            };
        }

        public async Task<DocumentContainer<TDocument>> UpsertAsync(
            string id,
            string etag,
            TDocument document,
            CancellationToken cancellationToken)
        {
            id.GuardNullOrWhiteSpace(nameof(id));
            document.Guard(nameof(document));

            var wrapped = Wrap(id, etag, document);
            return await UpsertInternalAsync(id, wrapped, cancellationToken);
        }

        public async Task<DocumentContainer<TDocument>> DeleteAsync(string id, CancellationToken cancellationToken)
        {
            id.GuardNullOrWhiteSpace(nameof(id));

            var document = await GetAsync(id, cancellationToken);
            document.State |= DocumentState.Deleted;
            return await UpsertInternalAsync(id, document, cancellationToken);
        }

        private async Task<DocumentContainer<TDocument>> UpsertInternalAsync(string id, DocumentContainer<TDocument> document, CancellationToken cancellationToken)
        {
            document.State |= DocumentState.Updated | DocumentState.Created;
            switch (ValidateDocument(nameof(UpsertAsync), id, document, out var validated))
            {
                case null: break;
                case { } e: throw e;
            }

            var requestOptions = GetRequestOptions(validated.ETag);
            var container = await GetContainerAsync();
            var response = await container.UpsertItemAsync(validated, _partitionKey, requestOptions, cancellationToken);
            return ValidateDocument(nameof(UpsertAsync), id, response, out validated) switch
            {
                null => validated,
                { } e => throw e
            };
        }

        private Exception ValidateDocument(string context, string id, ItemResponse<DocumentContainer<TDocument>> toValidate, out DocumentContainer<TDocument> validated)
        {
            validated = null;
            if (toValidate == null)
            {
                return new InternalServerErrorException($"{nameof(RepositoryPartition<TDocument>)}, document {_partitionKeyValue}/{id}: null database response");
            }

            switch (toValidate.StatusCode)
            {
                case HttpStatusCode.NotFound:
                    return new NotFoundException($"{context}, document {_partitionKeyValue}/{id}: document not found");
                case HttpStatusCode.InternalServerError:
                    return new InternalServerErrorException($"{context}, document {_partitionKeyValue}/{id}: operation failed");
                case { } code when code >= HttpStatusCode.MultipleChoices:
                    return new HttpException(toValidate.StatusCode, $"{context}, document {_partitionKeyValue}/{id}: encountered HTTP {(int)code} - {code.ToName()}");
            }

            if (toValidate.Resource.ETag.IsNullOrWhiteSpace())
            {
                toValidate.Resource.ETag ??= toValidate.ETag;
            }

            return ValidateDocument(context, id, toValidate.Resource, out validated);
        }

        private Exception ValidateDocument(string context, string id, DocumentContainer<TDocument> toValidate, out DocumentContainer<TDocument> validated)
        {
            validated = null;
            if (toValidate == null)
            {
                return new NotFoundException($"{context}, document {_partitionKeyValue}/{id}: document not found");
            }

            switch (toValidate.State)
            {
                case DocumentState.Faulted:
                    throw new DocumentFaultException($"{context}, document {_partitionKeyValue}/{id}: document is in a faulted state, please see logs for details");
            }

            // Driven by config
            toValidate.PartitionKey ??= _partitionKeyValue;
            toValidate.Id ??= id;

            validated = toValidate;
            return null;
        }

        private ItemRequestOptions GetRequestOptions(string etag = null)
        {
            return new()
            {
                IfMatchEtag = etag
            };
        }

        private DocumentContainer<TDocument> Wrap(string id, string etag, TDocument document)
        {
            return new()
            {
                Value = document,
                Id = id,
                ETag = etag,
                PartitionKey = _partitionKeyValue
            };
        }

        private async Task<TType> TryAsync<TType>(Func<Task<TType>> operation)
        {
            const string wrapperError = nameof(RepositoryPartition<TDocument>) + ": operation failed, see inner exception for details";
            try
            {
                return await operation();
            }
            catch (CosmosException ce) when (ce.StatusCode == HttpStatusCode.BadRequest)
            {
                throw new BadRequestException(wrapperError, ce);
            }
            catch (CosmosException ce) when (ce.StatusCode == HttpStatusCode.NotFound)
            {
                throw new NotFoundException(wrapperError, ce);
            }
            catch (CosmosException ce) when (ce.StatusCode == HttpStatusCode.InternalServerError)
            {
                throw new InternalServerErrorException(wrapperError, ce);
            }
            catch (CosmosException ce)
            {
                throw new HttpException(ce.StatusCode, wrapperError, ce);
            }
        }
    }
}