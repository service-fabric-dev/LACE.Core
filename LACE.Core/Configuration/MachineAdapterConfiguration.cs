// Root myDeserializedClass = JsonConvert.DeserializeObject(myJsonResponse); 

using LACE.Core.Abstractions.Configuration;

namespace LACE.Core.Configuration
{
    public class MachineAdapterConfiguration : IMachineAdapterConfiguration
    {
        public string MachineType { get; set; }
    }
}