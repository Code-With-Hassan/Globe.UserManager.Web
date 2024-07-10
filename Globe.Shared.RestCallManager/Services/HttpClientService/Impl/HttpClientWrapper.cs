using System.Net.Http.Headers;
using Globe.Shared.RestCallManager.Services.TokenManagerService;

namespace Globe.Shared.RestCallManager.Services.HttpClientService.Impl
{
    public class HttpClientWrapper : IHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenManager _tokenManager;

        public HttpClientWrapper(ITokenManager tokenManager)
        {
            _httpClient = new HttpClient();
            _tokenManager = tokenManager;
        }

        private void AddAuthorizationHeader(bool isPublic)
        {
            if (!isPublic)
                return;

            var token = _tokenManager.GetTokenFromCookies();
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<HttpResponseMessage> GetAsync(string url, bool isPublic = true)
        {
            AddAuthorizationHeader(isPublic);
            return await _httpClient.GetAsync(url);
        }

        public async Task<HttpResponseMessage> PostAsync(string url, HttpContent content, bool isPublic = true)
        {
            AddAuthorizationHeader(isPublic);
            return await _httpClient.PostAsync(url, content);
        }

        public async Task<HttpResponseMessage> PutAsync(string url, HttpContent content, bool isPublic = true)
        {
            AddAuthorizationHeader(isPublic);
            return await _httpClient.PutAsync(url, content);
        }

        public async Task<HttpResponseMessage> DeleteAsync(string url, bool isPublic = true)
        {
            AddAuthorizationHeader(isPublic);
            return await _httpClient.DeleteAsync(url);
        }
    }
}
