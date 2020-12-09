using Autofac;

namespace LACE.Core.Autofac
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder AddCoreModules(this ContainerBuilder builder)
        {
            builder.RegisterModule(new CoreModule());

            return builder;
        }
    }
}
