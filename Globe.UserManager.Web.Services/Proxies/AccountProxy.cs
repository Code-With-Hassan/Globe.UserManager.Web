using Globe.Shared.RestCallManager.Models;

namespace Globe.UserManager.Web.Services.Repositories
{
    public record LoginAsync(string Url = "Auth/Login", bool IsPublic = false) : IProxy;

}
