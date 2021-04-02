namespace LACE.Core.Abstractions.Configuration
{
    // ReSharper disable once TypeParameterCanBeVariant
    public interface IConfigurationLoader
    {
        TConfiguration Load<TConfiguration>() where TConfiguration : IConfiguration;
    }
}