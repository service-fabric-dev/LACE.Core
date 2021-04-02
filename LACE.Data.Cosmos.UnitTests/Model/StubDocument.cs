using LACE.Data.Cosmos.Abstractions;
using Newtonsoft.Json;

namespace LACE.Data.Cosmos.UnitTests.Model
{
    public class StubDocument : IEtagged
    {
        public string StubProperty { get; set; }

        [JsonIgnore]
        public string ETag { get; set; }
    }
}
