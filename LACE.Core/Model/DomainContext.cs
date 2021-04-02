using LACE.Core.Abstractions.Model;
using LACE.Core.Extensions;

namespace LACE.Core.Model
{
    public class DomainContext : IDomainContext
    {
        public DomainContext(
            IMachines machines,
            IMeters meters,
            IMonitors monitors)
        {
            Machines = machines.Guard(nameof(machines));
            Meters = meters.Guard(nameof(meters));
            Monitors = monitors.Guard(nameof(monitors));
        }

        public IMachines Machines { get; }
        public IMeters Meters { get; }
        public IMonitors Monitors { get; }
    }
}
