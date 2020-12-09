using System.Threading.Tasks;

namespace LACE.Core.Abstractions.Model
{
    public interface IRoutine
    {
        Task<IRoutineReport> ExecuteAsync();
    }
}
