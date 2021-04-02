using LACE.Data.Cosmos.Model;
using System.Threading;
using System.Threading.Tasks;

namespace LACE.Data.Cosmos.Abstractions
{
    public interface IDocumentRepository<TDocument>
    {
        Task<DocumentContainer<TDocument>> GetAsync(string    id, CancellationToken cancellationToken);
        Task<DocumentContainer<TDocument>> UpsertAsync(string id, string etag, TDocument document, CancellationToken cancellationToken);
        Task<DocumentContainer<TDocument>> DeleteAsync(string id, CancellationToken cancellationToken);
    }
}