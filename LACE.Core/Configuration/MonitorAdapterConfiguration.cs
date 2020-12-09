using LACE.Core.Abstractions.Configuration;

namespace LACE.Core.Configuration
{
    public class MonitorAdapterConfiguration : IMonitorAdapterConfiguration
    {
        public string MonitorType => GetType().Name;
    }
}