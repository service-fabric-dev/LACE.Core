using System.Collections.Generic;

namespace LACE.Data.Cosmos.Configuration
{
    public class RepositoryPartitionConfigurations : Dictionary<string, RepositoryPartitionConfiguration>
    {
        public RepositoryPartitionConfiguration Default => ContainsKey(nameof(Default)) ? this[nameof(Default)] : this[nameof(Default)] = new RepositoryPartitionConfiguration();
    }
}
