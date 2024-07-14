using Globe.Shared.Helpers;
using System.Text.Json.Serialization;

namespace Globe.Shared.RestCallManager.Models
{
    public class Response<T>
    {
        public bool Success { get; set; } = false;
        public T Data { get; set; } = default;
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
