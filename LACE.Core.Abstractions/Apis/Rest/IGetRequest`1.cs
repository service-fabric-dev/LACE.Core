namespace LACE.Core.Abstractions.Apis.Rest
{
    public interface IGetRequest<T>
    {
        string GetUri { get; }
    }
}