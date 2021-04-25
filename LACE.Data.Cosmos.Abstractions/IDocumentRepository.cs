using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LACE.Data.Cosmos.Model;

namespace LACE.Data.Cosmos.Abstractions
{
    public interface IDocumentRepository<TDocument>
    {
        Task<List<DocumentContainer<TDocument>>> GetAllAsync(CancellationToken cancellationToken);
        Task<DocumentContainer<TDocument>> GetAsync(string id, CancellationToken cancellationToken);
        Task<DocumentContainer<TDocument>> UpsertAsync(string id, string etag, TDocument document, CancellationToken cancellationToken);
        Task<DocumentContainer<TDocument>> DeleteAsync(string id, CancellationToken cancellationToken);
    }
}