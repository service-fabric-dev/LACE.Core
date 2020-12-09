namespace LACE.Core.Abstractions.Apis.Rest
{
    public interface IPostRequest<T>
    {
        string PostUri { get; }
    }
}