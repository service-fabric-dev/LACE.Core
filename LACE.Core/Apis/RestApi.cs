using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LACE.Core.Abstractions.Apis.Rest;
using LACE.Core.Extensions;

namespace LACE.Core.Apis
{
    public class RestApi : IRestAdapter
    {
        private readonly HttpClient _httpClient;

        public RestApi(HttpClient httpClient)
        {
            _httpClient = httpClient.Guard(nameof(httpClient));
        }

        public Task<TResponse> GetAsync<TResponse>(IGetRequest<TResponse> request)
        {
            throw new NotImplementedException();
        }

        public Task<TResponse> PutAsync<TResponse>(IPutRequest<TResponse> request)
        {
            throw new NotImplementedException();
        }

        public Task<TResponse> PostAsync<TResponse>(IPostRequest<TResponse> request)
        {
            throw new NotImplementedException();
        }
    }
}
