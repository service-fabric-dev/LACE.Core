namespace LACE.Core.Abstractions.Configuration
{
    // ReSharper disable once TypeParameterCanBeVariant
    public interface IConfigurationLoader
    {
        TConfiguration Load<TConfiguration>(string key) where TConfiguration : new();
    }
}