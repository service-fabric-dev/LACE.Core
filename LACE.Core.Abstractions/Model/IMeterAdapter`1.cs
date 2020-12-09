using System.Threading.Tasks;

namespace LACE.Core.Abstractions.Model
{
    public interface IMeterAdapter<TMetric> : IMeterAdapter
        where TMetric : IFact
    {
        Task<TMetric> ReadConcreteAsync();
    }
}
