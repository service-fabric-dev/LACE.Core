using System.Threading;
using System.Threading.Tasks;

namespace LACE.Core.Abstractions.Model
{
    public interface IMeterAdapter
    {
        Task<IFact> ReadAsync(CancellationToken cancellation);
    }
}
