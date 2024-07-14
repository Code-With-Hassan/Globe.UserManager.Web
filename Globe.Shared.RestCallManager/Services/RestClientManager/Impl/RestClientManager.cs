using System.Text.Json;
using System.Text;
using Globe.Shared.RestCallManager.Services.HttpClientService;
using Globe.Shared.RestCallManager.Models;

namespace Globe.Shared.RestCallManager.Services.RestClientManager.Impl
{
    public class RestClientManager : IRestClientManager
    {
        private readonly IHttpClient _httpClient;

        public RestClientManager(IHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Response<T>> GetAsync<T>(string url)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string content = await response.Content.ReadAsStringAsync();
                T data = JsonSerializer.Deserialize<T>(content);
                return new Response<T> { Success = true, Data = data };
            }
            catch (Exception ex)
            {
                return new Response<T> { Success = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<Response<T>> PostAsync<T, K>(string url, K requestData)
        {
            try
            {
                string json = JsonSerializer.Serialize(requestData);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PostAsync(url, content);
                response.EnsureSuccessStatusCode();
                string responseData = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                return JsonSerializer.Deserialize<Response<T>>(responseData, options);
            }
            catch (Exception ex)
            {
                return new Response<T> { Success = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<Response<T>> PutAsync<T, K>(string url, K requestData)
        {
            try
            {
                string json = JsonSerializer.Serialize(requestData);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PutAsync(url, content);
                response.EnsureSuccessStatusCode();
                string responseData = await response.Content.ReadAsStringAsync();
                T data = JsonSerializer.Deserialize<T>(responseData);
                return new Response<T> { Success = true, Data = data };
            }
            catch (Exception ex)
            {
                return new Response<T> { Success = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<Response<T>> DeleteAsync<T>(string url)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.DeleteAsync(url);
                response.EnsureSuccessStatusCode();
                string content = await response.Content.ReadAsStringAsync();
                T data = JsonSerializer.Deserialize<T>(content);
                return new Response<T> { Success = true, Data = data };
            }
            catch (Exception ex)
            {
                return new Response<T> { Success = false, ErrorMessage = ex.Message };
            }
        }
    }
}
