using System.Collections.Generic;
using LACE.Core.Abstractions.Model;

namespace LACE.Core.Model
{
    public class Meters : Dictionary<string, IMeterAdapter>, IMeters
    {
    }
}
