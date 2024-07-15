using System.Text.Json;
using System.Text;
using Globe.Shared.RestCallManager.Services.HttpClientService;
using Globe.Shared.RestCallManager.Models;

namespace Globe.Shared.RestCallManager.Services.RestClientManager.Impl
{
    public class RestClientManager : IRestClientManager
    {
        private JsonSerializerOptions _jsonSerializerOptions;
        private readonly string _contentType = "application/json";
        private readonly IHttpClient _httpClient;

        public RestClientManager(IHttpClient httpClient)
        {
            _httpClient = httpClient;
            _jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<Response<T>> PostAsync<T, K, P>(P proxy, K requestData) where P : IProxy
        {
            try
            {
                string json = JsonSerializer.Serialize(requestData);
                StringContent content = new StringContent(json, Encoding.UTF8, _contentType);

                HttpResponseMessage response = await _httpClient.PostAsync(proxy.Url, content, proxy.IsPublic);
                //response.EnsureSuccessStatusCode();

                string responseData = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Response<T>>(responseData, _jsonSerializerOptions) ?? new();
            }
            catch (Exception ex)
            {
                return new Response<T> { Success = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<Response<T>> PutAsync<T, K, P>(P proxy, K requestData) where P : IProxy
        {
            try
            {
                string json = JsonSerializer.Serialize(requestData);
                StringContent content = new StringContent(json, Encoding.UTF8, _contentType);

                HttpResponseMessage response = await _httpClient.PutAsync(proxy.Url, content, proxy.IsPublic);
                response.EnsureSuccessStatusCode();

                string responseData = await response.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<Response<T>>(responseData, _jsonSerializerOptions) ?? new();
            }
            catch (Exception ex)
            {
                return new Response<T> { Success = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<Response<T>> DeleteAsync<T, P>(P proxy) where P : IProxy
        {
            try
            {
                HttpResponseMessage response = await _httpClient.DeleteAsync(proxy.Url, proxy.IsPublic);

                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<Response<T>>(content, _jsonSerializerOptions) ?? new();
            }
            catch (Exception ex)
            {
                return new Response<T> { Success = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<Response<T>> GetAsync<T, P>(P proxy) where P : IProxy
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(proxy.Url, proxy.IsPublic);
                response.EnsureSuccessStatusCode();

                var stream = await response.Content.ReadAsStreamAsync();
                string content = await response.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<Response<T>>(content, _jsonSerializerOptions) ?? new();
            }
            catch (Exception ex)
            {
                return new Response<T> { Success = false, ErrorMessage = ex.Message };
            }
        }
    }
}
