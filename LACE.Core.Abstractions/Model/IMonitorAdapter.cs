using System.Threading;
using System.Threading.Tasks;

namespace LACE.Core.Abstractions.Model
{
    public interface IMonitorAdapter
    {
        bool IsTriggered(IFacts facts);
        Task WriteAsync(CancellationToken cancellation);
    }
}
