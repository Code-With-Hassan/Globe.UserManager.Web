using Globe.Shared.RestCallManager.Models;

namespace Globe.Shared.RestCallManager.Services.RestClientManager
{
    public interface IRestClientManager
    {
        Task<Response<T>> GetAsync<T, P>(P proxy) where P : IProxy;
        Task<Response<T>> PostAsync<T, K, P>(P proxy, K requestData) where P : IProxy;
        Task<Response<T>> PutAsync<T, K, P>(P proxy, K requestData) where P : IProxy;
        Task<Response<T>> DeleteAsync<T, P>(P proxy) where P : IProxy;
    }
}
