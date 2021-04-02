using System.Collections.Generic;
using LACE.Core.Abstractions.Model;

namespace LACE.Core.Model
{
    public class Monitors : Dictionary<string, IMonitorAdapter>, IMonitors
    {
    }
}
