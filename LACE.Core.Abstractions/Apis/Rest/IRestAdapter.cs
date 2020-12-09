using System.Threading.Tasks;

namespace LACE.Core.Abstractions.Apis.Rest
{
    public interface IRestAdapter
    {
        Task<TResponse> GetAsync<TResponse>(IGetRequest<TResponse> request);
        Task<TResponse> PutAsync<TResponse>(IPutRequest<TResponse> request);
        Task<TResponse> PostAsync<TResponse>(IPostRequest<TResponse> request);
    }
}
