using System;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Converters;

namespace LACE.Data.Cosmos.Enums
{
    [Flags]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum DocumentState : short
    {
        Unknown = 0,
        Created = 1,
        Updated = 2,
        Deleted = 4,
        Faulted = 8
    }
}
