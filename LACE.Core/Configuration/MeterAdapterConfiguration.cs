// Root myDeserializedClass = JsonConvert.DeserializeObject(myJsonResponse); 

using LACE.Core.Abstractions.Configuration;

namespace LACE.Core.Configuration
{
    public class MeterAdapterConfiguration : IMeterAdapterConfiguration
    {
        public string MeterType { get; set; }
    }
}