namespace Globe.Shared.RestCallManager.Services.HttpClientService
{
    public interface IHttpClient
    {
        Task<HttpResponseMessage> GetAsync(string url, bool isPublic = true);
        Task<HttpResponseMessage> PostAsync(string url, HttpContent content, bool isPublic = true);
        Task<HttpResponseMessage> PutAsync(string url, HttpContent content, bool isPublic = true);
        Task<HttpResponseMessage> DeleteAsync(string url, bool isPublic = true);
    }

}
