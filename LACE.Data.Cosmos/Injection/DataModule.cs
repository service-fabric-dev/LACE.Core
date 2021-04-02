using LACE.Core.Abstractions.Configuration;
using LACE.Core.Abstractions.Injection;
using LACE.Core.Exceptions;
using LACE.Core.Extensions;
using LACE.Data.Cosmos.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LACE.Data.Cosmos.Injection
{
    public class DataModule : IModule
    {
        private readonly IConfigurationLoader _configurationLoader;

        public DataModule(IConfigurationLoader configurationLoader)
        {
            _configurationLoader = configurationLoader.Guard(nameof(configurationLoader));
        }
        
        public void Register(IServiceCollection services)
        {
            var dataConfiguration = _configurationLoader.Load<DataConfiguration>();
            if (dataConfiguration == null)
            {
                throw new InjectionException();
            }

            services.AddSingleton(dataConfiguration);

            //services.AddScoped<CardRepository>();
        }
    }
}
