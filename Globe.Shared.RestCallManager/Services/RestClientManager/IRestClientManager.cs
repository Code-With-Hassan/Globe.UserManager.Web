using Globe.Shared.Models;

namespace Globe.Shared.RestCallManager.Services.RestClientManager
{
    public interface IRestClientManager
    {
        Task<Response<T>> GetAsync<T>(string url);
        Task<Response<T>> PostAsync<T, K>(string url, K requestData);
        Task<Response<T>> PutAsync<T, K>(string url, K requestData);
        Task<Response<T>> DeleteAsync<T>(string url);
    }
}
