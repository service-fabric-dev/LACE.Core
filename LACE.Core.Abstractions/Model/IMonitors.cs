namespace LACE.Core.Abstractions.Model
{
    public interface IMonitors
    {
        IMonitorAdapter this[string id]
        {
            get;
        }
    }
}
