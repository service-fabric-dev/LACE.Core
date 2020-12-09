using System.Threading;
using System.Threading.Tasks;

namespace LACE.Core.Abstractions.Model
{
    public interface IMachineAdapter
    {
        Task WorkAsync(IFacts facts, CancellationToken cancellation);
    }
}
