using LACE.Core.Abstractions.Configuration;
using LACE.Core.Extensions;
using Microsoft.Extensions.Configuration;

namespace LACE.Core.Configuration
{
    public class BindingConfigurationLoader : IConfigurationLoader
    {
        private readonly IConfiguration _configuration;

        public BindingConfigurationLoader(IConfiguration configuration)
        {
            _configuration = configuration.Guard(nameof(configuration));
        }

        public TConfiguration Load<TConfiguration>(string key) where TConfiguration : new()
        {
            var config = new TConfiguration();
            _configuration.Bind(key, config);
            return config;
        }
    }
}
