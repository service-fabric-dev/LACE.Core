using System.Threading;
using System.Threading.Tasks;
using LACE.Data.Cosmos.Abstractions;
using LACE.Data.Cosmos.Model;

namespace LACE.Data.Cosmos
{
    public interface ISimpleRepository<TDocument> where TDocument : class, IEtagged
    {
        Task<DocumentContainer<TDocument>> GetAsync(string id, CancellationToken cancellationToken);
        Task<DocumentContainer<TDocument>> UpsertAsync(string id, TDocument document, CancellationToken cancellationToken);
        Task<DocumentContainer<TDocument>> DeleteAsync(string id, CancellationToken cancellationToken);
    }
}